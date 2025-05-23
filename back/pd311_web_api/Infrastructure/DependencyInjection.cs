using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using pd311_web_api.BLL.Services.Account;
using pd311_web_api.BLL.Services.Cars;
using pd311_web_api.BLL.Services.Email;
using pd311_web_api.BLL.Services.Image;
using pd311_web_api.BLL.Services.JwtService;
using pd311_web_api.BLL.Services.Manufactures;
using pd311_web_api.BLL.Services.Role;
using pd311_web_api.BLL.Services.Storage;
using pd311_web_api.BLL.Services.User;
using Quartz;
using System.Reflection;
using System.Text;

namespace pd311_web_api.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddJwtSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidAudience = configuration["JwtSettings:Audience"],
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"] ?? "")),
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        public static void AddServices(this IServiceCollection services, Assembly[] assemblies)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IManufactureService, ManufactureService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IStorageService, StorageService>();
        }

        public static void AddJobs(this IServiceCollection services, params (Type type, string schedule)[] jobs)
        {
            services.AddQuartz(q =>
            {
                foreach (var item in jobs)
                {
                    var jobKey = new JobKey(item.type.Name);
                    q.AddJob(item.type, jobKey);

                    q.AddTrigger(opt => opt
                    .ForJob(jobKey)
                    .WithIdentity($"{item.type.Name}-trigger")
                    .WithCronSchedule(item.schedule));
                }
            });
        }
    }
}
