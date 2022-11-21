namespace Domain.DTOs;

public class ProjectCreateDTO
{
    public string Name { get; }
    public int TeamId { get; }
    public DateTime Deadline { get; }

    public ProjectCreateDTO(string name, int teamId, DateTime deadline)
    {
        Name = name;
        TeamId = teamId;
        Deadline = deadline;
    }
    
}