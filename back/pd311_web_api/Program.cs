using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using pd311_web_api.BLL;
using pd311_web_api.BLL.DTOs.Account;
using pd311_web_api.DAL;
using pd311_web_api.DAL.Initializer;
using pd311_web_api.DAL.Repositories.Cars;
using pd311_web_api.DAL.Repositories.JwtRepository;
using pd311_web_api.DAL.Repositories.Manufactures;
using pd311_web_api.Infrastructure;
using pd311_web_api.Jobs;
using pd311_web_api.Middlewares;
using Quartz;
using Serilog;
using StackExchange.Redis;
using static pd311_web_api.DAL.Entities.IdentityEntities;

var builder = WebApplication.CreateBuilder(args);

// Logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log_.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add jwt
builder.Services.AddJwtSettings(builder.Configuration);

// Add services to the container.
builder.Services.AddServices(AppDomain.CurrentDomain.GetAssemblies());

// Add jobs
var jobs = new (Type type, string schedule)[]
{
    //(typeof(HelloJob), "0 0/1 * * * ?"),
    (typeof(CleanLogsJob), "* 0 0 * * ?")
    //(typeof(MailingJob), "* 0 12 29 2 ?")
};

builder.Services.AddJobs(jobs);
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

// redis
//builder.Services.AddScoped(cfg =>
//{
//    IConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect($"localhost");
//    return multiplexer.GetDatabase();
//});

// Add repositories
builder.Services.AddScoped<IManufactureRepository, ManufactureRepository>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IJwtRepository, JwtRepository>();

builder.Services.AddControllers();

// Add fluent validation
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

// Add automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("NpgsqlAzure"));
});

// Add identity
builder.Services
    .AddIdentity<AppUser, AppRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// CORS
const string corsPolicyName = "CorsPolicy";
var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

if(origins == null || origins.Length == 0)
{
    origins = ["http://localhost"]; 
}

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, builder =>
    {
        builder
        .WithOrigins(origins)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

// JWT bearer authorization
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PD311_API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new []{Settings.RoleAdmin}
        }
    });
});

var app = builder.Build();

// Middlewares
app.UseMiddleware<MiddlewareExceptionHandler>();
app.UseMiddleware<MiddlewareNullExceptionHandler>();
app.UseMiddleware<MiddlewareLogger>();

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    //app.MapOpenApi();
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();


Settings.RootPath = builder.Environment.ContentRootPath;
string rootPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
string imagesPath = Path.Combine(rootPath, Settings.RootImagesPath);

if (!Directory.Exists(rootPath))
{
    Directory.CreateDirectory(rootPath);
}

if (!Directory.Exists(imagesPath))
{
    Directory.CreateDirectory(imagesPath);
}

// static files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesPath),
    RequestPath = "/images"
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Seed();

app.Run();