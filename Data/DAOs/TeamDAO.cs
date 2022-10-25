using Application.DAOInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace Data.DAOs;


//TODO: Revise irrelevant methods
public class TeamDAO : ITeamDAO
{
    private readonly FileContext context;

    public TeamDAO(FileContext context)
    {
        this.context = context;
    }
    public Task<Team> CreateAsync(Team team)
    {
        int teamId = 1;

        if (context.Teams.Any())
        {
            teamId = context.Teams.Max(u => u.Id);
            teamId++;
        }
        team.Id = teamId;

        context.Teams.Add(team);
        context.SaveChanges();

        return Task.FromResult(team);
    }

    public Task DeleteAsync(Team team)
    {
        Team? existing = context.Teams.FirstOrDefault(t => t.Id == team.Id);

        if (existing == null)
        {
            throw new Exception($"Team with id{team.Id} has not been found");
        }

        context.Teams.Remove(existing);
        context.SaveChanges();
        return Task.CompletedTask;
    }
    
    public Task UpdateAsync(Team team)
    {
        Team? existing = context.Teams.FirstOrDefault(u => u.Id == team.Id);

        if (existing == null)
        {
            throw new Exception($"User with id {team.Id} has not been found");
        }

        context.Teams.Remove(existing);
        context.Teams.Add(team);
        context.SaveChanges();

        return Task.CompletedTask;
    }
    public Task<Team?> GetByName(string name)
    {
        Team? existing = context.Teams.FirstOrDefault(u => u.Name.Equals(name));
        return Task.FromResult(existing);
    }

    public Task<Team?> GetByIdAsync(int id)
    {
        Team? existing = context.Teams.FirstOrDefault(u => u.Id.Equals(id));
        return Task.FromResult(existing);
    }

    public Task<IEnumerable<Team>> GetByUserIdAsync(int id)
    {
        IEnumerable<Team> teams = new List<Team>();

        teams = teams.Concat(context.Teams.Where(s => s.TeamLeader.Id == id));
        teams = teams.Concat(context.Teams.Where(s => s.members.Any(u => u.Id == id)));

        return Task.FromResult(teams);
    }
}