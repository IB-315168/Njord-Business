using Domain.DTOs.Task;
using Domain.Models;

namespace Application.DAOInterfaces;

public interface ITaskDAO
{
    Task<TaskEntity> CreateAsync(TaskCreateDTO dto);
    Task<Task> UpdateAsync(TaskUpdateDTO dto);
    Task<Task> DeleteAsync(int id);
    Task<TaskEntity> GetByIdAsync(int id);
    Task<ICollection<TaskEntity>> GetByProjectIdAsync(int id);
}