using Domain.DTOs;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface ITeamLogic
{
    Task<Team> CreateAsync(TeamCreateDTO dto);
    Task<TeamBasicDTO> GetByIdAsync(int id);
    Task<TeamBasicDTO> GetByUserIdAsync(int id);
}