using Domain.DTOs;
using Domain.Models;

namespace Application.DAOInterfaces;

//TODO: Revise
public interface ITeamDAO
{
    Task<Team> CreateAsync(Team team);
    Task DeleteAsync(Team team);
    Task UpdateAsync(Team team);
    
    Task<Team> GetByName(string name);
    Task<Team?> GetByIdAsync(int id);
    Task<IEnumerable<Team>> GetByUserIdAsync(int id);
}