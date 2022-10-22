﻿using Application.DAOInterfaces;
using Domain.Models;

namespace Data.DAOs;

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
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Team team)
    {
        throw new NotImplementedException();
    }
    public Task<Team?> GetByName(string name)
    {
        Team? existing = context.Teams.FirstOrDefault(u => u.Name.Equals(name));
        return Task.FromResult(existing);
    }

    public Task<Team> GetByIdAsync(int id)
    {
        Team? existing = context.Teams.FirstOrDefault(u => u.Id.Equals(id));
        return Task.FromResult(existing);
    }

    public Task<Team> GetByUserIdAsync(int id)
    {
        Team? existing = context.Teams.FirstOrDefault(u => u.TeamLeaderId.Equals(id));
        return Task.FromResult(existing);
    }
}