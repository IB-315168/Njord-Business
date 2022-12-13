namespace Domain.DTOs.Meeting;

public class MeetingCreateDTO
{
    public int AssignedLeader { get; set; }
    public int ProjectAssigned { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}