using Application.Features.Restaurants.Models;

namespace Application.Features.Restaurants.Commands.AddWorkHours
{
    public class AddWorkHoursCommand : ICommand<int>
    {
        public required int RestaurantId { get; set; }
        public ICollection<WorkHourDto> WorkHours { get; set; } = new List<WorkHourDto>();
    }
}
