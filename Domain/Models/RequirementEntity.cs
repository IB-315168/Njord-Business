namespace Domain.Models;

public class RequirementEntity : IEquatable<RequirementEntity>
{
    public int Id { get; set; }
    public int IdProject { get; set; }
    public string content { get; set; }

    public RequirementEntity(int id, int idProject, string content)
    {
        Id = id;
        IdProject = idProject;
        this.content = content;
    }

    public bool Equals(RequirementEntity? other)
    {
        if(other == null)
        {
            return false;
        }

        return Id == other.Id && IdProject == other.IdProject && content.Equals(other.content);
    }
}