using Domain.DTOs.Project;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface IProjectLogic
{
    Task<ProjectEntity> CreateAsync(ProjectCreateDTO dto);
    Task DeleteAsync(int id);
    Task UpdateAsync(ProjectUpdateDTO dto);

    Task<ProjectEntity> GetByIdAsync(int id);
    Task<ICollection<BasicProjectDTO>> GetByMemberIdAsync(int id);
}