using Domain.Entities.Restaurants;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Users;

public abstract class ApplicationUser : IdentityUser<Guid>
{
    public abstract string MainRole { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
}

public class Admin : ApplicationUser
{
    public override string MainRole { get; set; } = Constants.Roles.Admin;
}

public class Customer : ApplicationUser
{
    public override string MainRole { get; set; } = Constants.Roles.Customer;
}

public class RestaurantOwner : ApplicationUser
{
    public override string MainRole { get; set; } = Constants.Roles.RestaurantOwner;

    public Restaurant? Restaurant { get; set; } = null!;
}