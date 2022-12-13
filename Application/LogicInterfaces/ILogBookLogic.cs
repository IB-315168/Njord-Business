using Domain.DTOs.LogBook;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface ILogBookLogic
{
    Task<LogBookEntity> CreateAsync(LogBookCreateDTO dto);
    Task UpdateAsync(LogBookUpdateDTO dto);
    Task DeleteAsync(int id);
    Task<LogBookEntity?> GetByIdAsync(int id);
    Task<LogBookEntity?> GetByProjectIdAsync(int id);
}