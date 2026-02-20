namespace Application.Features.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommand : ICommand<int>
    {
        public required int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
