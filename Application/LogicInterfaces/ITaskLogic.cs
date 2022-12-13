using Domain.DTOs.Task;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface ITaskLogic
{
    Task<TaskEntity> CreateAsync(TaskCreateDTO dto);
    Task UpdateAsync(TaskUpdateDTO dto);
    Task DeleteAsync(int id);
    Task<TaskEntity> GetByIdAsync(int id);
    Task<ICollection<TaskEntity>> GetByProjectIdAsync(int id);
}