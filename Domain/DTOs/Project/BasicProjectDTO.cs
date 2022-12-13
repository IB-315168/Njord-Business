namespace Domain.DTOs.Project;

public class BasicProjectDTO
{
    public int Id { get; }
    public string ProjectName { get; }
    public string TeamName { get; }

    public BasicProjectDTO(int id, string projectName, string teamName)
    {
        Id = id;
        ProjectName = projectName;
        TeamName = teamName;
    }
}