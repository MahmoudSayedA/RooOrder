namespace Domain.Entities.Restaurants;
public class RestaurantWorkingHours
{
    public int Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    public bool IsClosed { get; set; } = false;

    public int RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; } = null!;

}
