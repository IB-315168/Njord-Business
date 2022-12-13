namespace Domain.DTOs.Meeting;

public class MeetingUpdateDTO
{
    public int Id { get; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public MeetingUpdateDTO(int id)
    {
        Id = id;
    }
}