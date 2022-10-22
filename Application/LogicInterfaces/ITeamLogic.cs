using Domain.DTOs;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface ITeamLogic
{
    Task<Team> CreateAsync(TeamCreateDTO dto);
    Task<Team> GetByIdAsync(int id);
    Task<IEnumerable<TeamBasicDTO>> GetByUserIdAsync(int id);
}