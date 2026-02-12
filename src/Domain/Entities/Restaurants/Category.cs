namespace Domain.Entities.Restaurants;

public class Category
{
    public  int Id { get; set; }
    public required string Name { get; set; }
    public string? ImageUrl { get; set; }
    public int RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; } = null!;
}
