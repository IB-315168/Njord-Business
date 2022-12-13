using Domain.DTOs.Project;
using Domain.Models;

namespace Application.DAOInterfaces;

public interface IProjectDAO
{
    Task<ProjectEntity> CreateAsync(ProjectCreateDTO dto);
    Task<Task> UpdateAsync(ProjectUpdateDTO dto);
    Task<Task> DeleteAsync(int id);
    Task<ProjectEntity> GetByIdAsync(int id);
    Task<ICollection<BasicProjectDTO>> GetByMemberIdAsync(int id);
}