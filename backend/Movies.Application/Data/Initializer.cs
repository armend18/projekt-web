using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.AuthModels;

namespace Movies.Application.Data
{
    public static class Initializer
    {
        public static async Task SeedRolesAsync(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                }
        }
    }
}