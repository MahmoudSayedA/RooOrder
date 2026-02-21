namespace Application.Features.Restaurants.Commands.AddMenuItem
{
    public class AddMenuItemCommand : ICommand
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public bool IsAvailable { get; set; } = true;
        public bool IsHidden { get; set; } = false;
    }
}
