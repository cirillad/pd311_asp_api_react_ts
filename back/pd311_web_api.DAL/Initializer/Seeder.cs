using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using static pd311_web_api.DAL.Entities.IdentityEntities;

namespace pd311_web_api.DAL.Initializer
{
    public static class Seeder
    {
        public static async void Seed(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

                await context.Database.MigrateAsync();

                if(!roleManager.Roles.Any())
                {
                    var adminRole = new AppRole { Name = "admin" };
                    var userRole = new AppRole { Name = "user" };

                    await roleManager.CreateAsync(adminRole);
                    await roleManager.CreateAsync(userRole);

                    var admin = new AppUser
                    {
                        Email = "admin@mail.com",
                        EmailConfirmed = true,
                        UserName = "admin"
                    };

                    var user = new AppUser
                    {
                        Email = "user@mail.com",
                        EmailConfirmed = true,
                        UserName = "user"
                    };

                    await userManager.CreateAsync(admin, "qwerty");
                    await userManager.CreateAsync(user, "qwerty");

                    await userManager.AddToRoleAsync(admin, "admin");
                    await userManager.AddToRoleAsync(user, "user");
                }
            }
        }
    }
}
