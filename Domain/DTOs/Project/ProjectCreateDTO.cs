namespace Domain.DTOs.Project;

public class ProjectCreateDTO
{
    public string Name { get; set; }
    public int TeamId { get; set; }
    public DateTime Deadline { get; set; }
}