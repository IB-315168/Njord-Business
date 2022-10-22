using Domain.Models;

namespace Application.DAOInterfaces;

public interface ITeamDAO
{
    Task<Team> CreateAsync(Team team);
    Task DeleteAsync(Team team);
    Task UpdateAsync(Team team);
    
    Task<Team> GetByName(string name);
    Task<Team> GetByIdAsync(int id);
    Task<Team?> GetByUserIdAsync(int id);
}