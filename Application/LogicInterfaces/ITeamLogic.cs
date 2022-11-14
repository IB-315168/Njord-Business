using Domain.DTOs;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface ITeamLogic
{
    Task<TeamEntity> CreateAsync(TeamCreateDTO dto);
    Task<TeamEntity> GetByIdAsync(int id);
    Task<IEnumerable<TeamBasicDTO>> GetByUserIdAsync(int id);
    Task DeleteAsync(int id);
    Task UpdateAsync(TeamUpdateDTO dto);
}