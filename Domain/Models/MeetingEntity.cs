namespace Domain.Models;

public class MeetingEntity
{
    public int Id { get; set; }
    public MemberEntity AssignedLeader { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public MeetingEntity(int id, MemberEntity assignedLeader, string title, string description, DateTime startDate,
        DateTime endDate)
    {
        Id = id;
        AssignedLeader = assignedLeader;
        Title = title;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
    }

    public bool Equals(MeetingEntity? other)
    {
        if (other == null)
            return false;

        return Id == other.Id && AssignedLeader == other.AssignedLeader && Title == other.Title &&
               Description == other.Description && StartDate == other.StartDate && EndDate == other.EndDate;
    }
}