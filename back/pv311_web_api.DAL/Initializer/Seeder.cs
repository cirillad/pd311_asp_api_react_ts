using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using pv311_web_api.DAL.Entities;

namespace pv311_web_api.DAL.Initializer
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
                    var roleAdmin = new AppRole { Name = "admin" };
                    var roleUser = new AppRole { Name = "user" };

                    var admin = new AppUser
                    {
                        Email = "admin@gmail.com",
                        UserName = "admin",
                        EmailConfirmed = true
                    };

                    var user = new AppUser
                    {
                        Email = "user@gmail.com",
                        UserName = "user",
                        EmailConfirmed = true
                    };

                    await roleManager.CreateAsync(roleAdmin);
                    await roleManager.CreateAsync(roleUser);

                    await userManager.CreateAsync(admin, "qwerty");
                    await userManager.CreateAsync(user, "qwerty");

                    await userManager.AddToRoleAsync(admin, "admin");
                    await userManager.AddToRoleAsync(user, "user");
                }
            }
        }
    }
}
