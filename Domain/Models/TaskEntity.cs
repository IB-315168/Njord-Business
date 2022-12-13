namespace Domain.Models;

public class TaskEntity : IEquatable<TaskEntity>
{
    public int Id { get; set; }
    public MemberEntity memberAssigned { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime TimeEstimation { get; set; }
    public DateTime CreationDate { get; set; }

    public TaskEntity(int id, string title, string description, string status, DateTime creationDate)
    {
        Id = id;
        Title = title;
        Description = description;
        Status = status;
        CreationDate = creationDate;
    }

    public bool Equals(TaskEntity? other)
    {
        if (other == null)
        {
            return false;
        }

        return Id == other.Id && memberAssigned == other.memberAssigned &&
               Title == other.Title && Description == other.Description &&
               Status == other.Status;
    }
}