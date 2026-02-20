using Domain.Constants;
using Domain.Entities.Restaurants;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Seeding;

public static class InitialiserExtensions
{
    public static async Task InitializeDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        if(await initialiser.InitialiseAsync())
        {
            await initialiser.SeedAsync();
        }
    }
}

public class ApplicationDbContextInitialiser(
    ILogger<ApplicationDbContextInitialiser> logger,
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager)
{
    public async Task<bool> InitialiseAsync()
    {
        try
        {
            // await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            return true;
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
        string[] allRoles = [Roles.Admin, Roles.Customer, Roles.RestaurantOwner];

        var existing = await context.Roles.Where(r => r.Name != null && allRoles.Contains(r.Name)).Select(r => r.Name).ToListAsync();
        var needToBeAdded = allRoles.Except(existing).ToList();

        foreach (var roleName in needToBeAdded)
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
        }

        // Default users
        var admin = new Admin { UserName = "admin1", Email = "admin@rooorder.com", EmailConfirmed = true };
        var customer = new Customer { UserName = "customer1", Email = "user@rooorder.com", EmailConfirmed = true };
        var restaurantOwner = new RestaurantOwner { UserName = "owner1", Email = "owner.kfc@rooorder.com", EmailConfirmed = true };

        if (!(await userManager.Users.AnyAsync(u => u.UserName == admin.UserName)))
        {
            var result = await userManager.CreateAsync(admin, "123456");
            if (result.Succeeded)
                await userManager.AddToRolesAsync(admin, new[] { Roles.Admin });

            if (!await userManager.Users.AnyAsync(u => u.UserName == customer.UserName))
            {
                await userManager.CreateAsync(customer, "123456");
                await userManager.AddToRoleAsync(customer, Roles.Customer);
            }
            if (!(await userManager.Users.AnyAsync(u => u.UserName == restaurantOwner.UserName)))
            {
                await userManager.CreateAsync(restaurantOwner, "123456");
                await userManager.AddToRoleAsync(restaurantOwner, Roles.RestaurantOwner);
            }
        }
        // add restaurant
        if (!(await context.Restaurants.AnyAsync()))
        {
            var restaurant = new Restaurant
            {
                Name = "kfc",
                Description = "fried chicken",
                City = "Nasr City",
                Region = "Abas El-Aqqad",
                IsActive = true,
                OwnerId = restaurantOwner.Id,
            };
            await context.Restaurants.AddAsync(restaurant);
            await context.SaveChangesAsync();
        }

    }
}
