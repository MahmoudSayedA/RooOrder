namespace Application.Features.Restaurants.Commands.CreateRestaurant;
public class CreateRestaurantCommand : ICommand
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Address { get; set; }
    public required string Region { get; set; }
    public required string City { get; set; }
    public bool IsActive { get; set; } = true;
}
