using Azure.Storage.Blobs;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using pv311_web_api.BLL;
using pv311_web_api.BLL.DTOs.Account;
using pv311_web_api.DAL;
using pv311_web_api.DAL.Entities;
using pv311_web_api.DAL.Initializer;
using pv311_web_api.DAL.Repositories.Cars;
using pv311_web_api.DAL.Repositories.JwtRepository;
using pv311_web_api.DAL.Repositories.Manufactures;
using pv311_web_api.Infrastructure;
using pv311_web_api.Jobs;
using pv311_web_api.Middlewares;
using Quartz;
using Serilog;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Logger
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log_.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add jwt
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"] ?? "")),
            ClockSkew = TimeSpan.Zero
        };
    });

// Add services to the container.
builder.Services.AddServices();

// Add blob service
builder.Services.AddScoped(cfg =>
{
    var client = new BlobServiceClient(builder.Configuration.GetConnectionString("AzureStorage"));
    return client;
});

// redis
//builder.Services.AddScoped(cfg =>
//{
//    IConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect("localhost");
//    return multiplexer.GetDatabase();
//});

// Add quartz
var jobs = new (Type type, string cronExpression)[]
{
    (typeof(ConsoleLogJob), "0 * * * * ?"),
    (typeof(LogCleanJob), "0 1 0 * * ?"),
    (typeof(EmailJob), "0 0 12 1 * ?")
};

builder.Services.AddJobs(jobs);
builder.Services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);

// Add repositories
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IManufactureRepository, ManufactureRepository>();
builder.Services.AddScoped<IJwtRepository, JwtRepository>();

builder.Services.AddControllers();

// Add fluent validation
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

// Add automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    string? connectionString = builder.Configuration.GetConnectionString("NpgSqlAzure");
    options.UseNpgsql(connectionString);
});

// Add identity
builder.Services
    .AddIdentity<AppUser, AppRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// CORS
string corsPolicy = "CORSPolicy";
string[]? allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();

if(allowedOrigins == null || allowedOrigins.Length == 0)
{
    allowedOrigins = ["http://localhost"];
}

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, builder =>
    {
        builder
        .WithOrigins(allowedOrigins)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PV311_API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        In = ParameterLocation.Header,
        Description = "Enter bearer JWT token"
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
            new []{ Settings.AdminRole }
        }
    });
});

var app = builder.Build();

// Middlewares
app.UseMiddleware<MiddlewareLogger>();
app.UseMiddleware<MiddlewareExceptionHandler>();
app.UseMiddleware<MiddlewareNullExceptionHandler>();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    //app.MapOpenApi();
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(corsPolicy);

// Static files
var rootPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
var imagesPath = Path.Combine(rootPath, Settings.ImagesPath);
Settings.FilesRootPath = builder.Environment.ContentRootPath;

if (!Directory.Exists(rootPath))
{
    Directory.CreateDirectory(rootPath);
}

if (!Directory.Exists(imagesPath))
{
    Directory.CreateDirectory(imagesPath);
}

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