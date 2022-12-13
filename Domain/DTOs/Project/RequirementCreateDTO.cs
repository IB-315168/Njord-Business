namespace Domain.DTOs.Project;

public class RequirementCreateDTO
{
    public int idproject { get; }
    public string content { get; }

    public RequirementCreateDTO(int idproject, string content)
    {
        this.idproject = idproject;
        this.content = content;
    }
}