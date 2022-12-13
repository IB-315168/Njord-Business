namespace Domain.Models;

public class AvailabilityEntity
{
    public int Id { get; set; }
    public int DayOfWeek { get; set; }
    public string memberName { get; set; }
    public DateTime startHour { get; set; }
    public DateTime endHour { get; set; }
}