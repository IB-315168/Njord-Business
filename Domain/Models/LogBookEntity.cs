namespace Domain.Models;

public class LogBookEntity : IEquatable<LogBookEntity>
{
    public int Id { get; set; }
    public ProjectEntity Project { get; set; }
    public ICollection<LogBookEntryEntity> Entries { get; set; }

    public LogBookEntity(int id, ProjectEntity project)
    {
        Id = id;
        Project = project;
    }

    public bool Equals(LogBookEntity? other)
    {
        if (other == null)
        {
            return false;
        }

        return Id == other.Id && Project == other.Project;
    }
}