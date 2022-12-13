using Domain.DTOs.LogBook;
using Domain.Models;

namespace Application.DAOInterfaces;

public interface ILogBookDAO
{
    Task<LogBookEntity> CreateAsync(LogBookCreateDTO dto);
    Task<Task> UpdateAsync(LogBookUpdateDTO dto);
    Task<Task> DeleteAsync(int it);
    Task<LogBookEntity> GetByIdAsync(int id);
    Task<LogBookEntity?> GetByProjectIdAsync(int id);
}