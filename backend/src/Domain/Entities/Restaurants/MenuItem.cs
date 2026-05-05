namespace Domain.Entities.Restaurants;

public class MenuItem : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }

    public bool IsAvailable { get; set; } = true;
    public bool IsHidden { get; set; } = false;

    public int RestaurantId { get; set; }
    public int? CategoryId { get; set; }

    public Restaurant? Restaurant { get; set; }
    public Category? Category { get; set; }

}
