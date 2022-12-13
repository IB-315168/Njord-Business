using Domain.Models;

namespace Domain.DTOs.Project;

public class ProjectUpdateDTO
{
    public int Id { get; }
    public string Name { get; set; }
    public DateTime deadline { get; set; }
    public ICollection<RequirementEntity> requirements { get; set; }

    public ProjectUpdateDTO(int id)
    {
        Id = id;
    }
}