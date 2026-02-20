namespace Application.Features.Restaurants.Models
{

    public class WorkHourDto
    {
        public required DayOfWeek DayOfWeek { get; set; }
        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? ClosingTime { get; set; }
        public bool IsClosed { get; set; } = false;
    }

}
