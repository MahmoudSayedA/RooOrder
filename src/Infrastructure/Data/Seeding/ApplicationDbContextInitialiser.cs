using Domain.Constants;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Seeding;

public static class InitialiserExtensions
{
    //public static async Task InitialiseDatabaseAsync(this IApplicationBuilder app)
    //{
    //    using var scope = app.ApplicationServices.CreateScope();

    //    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

    //    await initialiser.InitialiseAsync();
    //    await initialiser.SeedAsync();
    //}
}

public class ApplicationDbContextInitialiser(
    ILogger<ApplicationDbContextInitialiser> logger,
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager)
{
    public async Task InitialiseAsync()
    {
        try
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        // Default roles
        var allRoles = typeof(Roles).GetMembers().Select(i => i.Name).ToList();

        var existing = await context.Roles.Where(r => r.Name != null && allRoles.Contains(r.Name)).Select(r => r.Name).ToListAsync();
        var needToBeAdded = allRoles.Except(existing).ToList();

        foreach (var roleName in needToBeAdded)
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
        }
         
        // Default users
        //var admin = new ApplicationUser { UserName = "admin@rooorder.com", Email = "admin@rooorder.com" };
        //var user = new ApplicationUser { UserName = "user@rooorder.com", Email = "user@rooorder.com" };

        //if (userManager.Users.All(u => u.UserName != admin.UserName))
        //{
        //    await userManager.CreateAsync(admin, "123456");
        //    await userManager.AddToRolesAsync(admin, new[] { Roles.Admin });

        //    if (!await userManager.Users.AnyAsync(u => u.UserName == user.UserName))
        //    {
        //        await userManager.CreateAsync(user, "123456");
        //        await userManager.AddToRoleAsync(user, Roles.User);
        //    }
        //}
        
    }
}
