using Domain.DTOs;
using Domain.Models;

namespace Application.DAOInterfaces;

public interface ITeamDAO
{
    Task<TeamEntity> CreateAsync(TeamEntity team);
    Task<Task> DeleteAsync(TeamEntity team);
    Task<Task> UpdateAsync(TeamEntity team);
    
    Task<TeamEntity?> GetByName(string name);
    Task<TeamEntity?> GetByIdAsync(int id);
}