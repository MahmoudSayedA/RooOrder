using Domain.Entities.Users;

namespace Domain.Entities.Restaurants;
public class Restaurant : BaseAuditableEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? Region { get; set; }
    public string? City { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid OwnerId { get; set; }

    public RestaurantOwner? Owner { get; set; } = null!;
    public List<RestaurantWorkingHours>? WorkingHours { get; set; } = null!;
    public List<Category>? Categories { get; set; } = null!;
    public List<MenuItem>? MenuItems { get; set; } = null!;

}
