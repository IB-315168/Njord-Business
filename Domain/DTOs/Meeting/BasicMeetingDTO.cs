namespace Domain.DTOs.Meeting;

public class BasicMeetingDTO
{
    public int Id { get; }
    public string Title { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    public BasicMeetingDTO(int id, string title, DateTime startDate, DateTime endDate)
    {
        Id = id;
        Title = title;
        StartDate = startDate;
        EndDate = endDate;
    }
}