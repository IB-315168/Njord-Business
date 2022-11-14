using Application.DAOInterfaces;
using Domain.DTOs;
using Domain.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcNjordClient;
using System.Xml.Linq;

namespace Data.DAOs;


//TODO: Revise irrelevant methods
public class TeamDAO : ITeamDAO
{
    private readonly TeamService.TeamServiceClient teamService;

    public TeamDAO()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:6565");
        this.teamService = new TeamService.TeamServiceClient(channel);
    }
    public async Task<TeamEntity> CreateAsync(TeamEntity team)
    {
        Team createdTeam = await teamService.CreateTeamAsync(new CreatingTeam() { Name = team.Name, TeamLeaderId = team.TeamLeader.Id });


        if(createdTeam == null)
        {
            throw new Exception("Failed to create team");
        }

        TeamEntity teamEntity = ConvertToTeamEntity(createdTeam);

        return teamEntity;
    }

    public async Task<Task> DeleteAsync(TeamEntity team)
    {
        TeamEntity? existing = await GetByIdAsync(team.Id);

        if (existing == null)
        {
            throw new Exception($"Team with id{team.Id} has not been found");
        }

        await teamService.DeleteTeamAsync(new Int32Value() { Value = team.Id });

        return Task.CompletedTask;
    }
    
    public async Task<Task> UpdateAsync(TeamEntity team)
    {
        TeamEntity? existing = await GetByIdAsync(team.Id);

        if (existing == null)
        {
            throw new Exception($"Team with id{team.Id} has not been found");
        }

        await teamService.UpdateTeamAsync(new UpdatingTeam()
        {
            Id = team.Id,
            Name = team.Name,
            TeamLeader = UserDAO.ConvertToUser(team.TeamLeader),
            Members = {UserDAO.ConvertToUsers(team.members)}
        });

        return Task.CompletedTask;
    }
    public async Task<TeamEntity?> GetByName(string name)
    {
        Team? reply = await teamService.GetByNameAsync(new StringValue() { Value = name });

        if (reply == null) {
            throw new Exception($"Team with name {name} has not been found.");
        }

        TeamEntity entity = ConvertToTeamEntity(reply);

        return entity;
    }

    public async Task<TeamEntity?> GetByIdAsync(int id)
    {
        Team? reply = await teamService.GetByIdAsync(new Int32Value { Value = id });

        if (reply == null)
        {
            throw new Exception($"Team with id {id} has not been found.");
        }

        TeamEntity entity = ConvertToTeamEntity(reply);

        return entity;
    }

    public static TeamEntity ConvertToTeamEntity(Team team)
    {
        return new TeamEntity()
        {
            Id = team.Id,
            Name = team.Name,
            TeamLeader = UserDAO.ConvertToUserEntity(team.TeamLeader),
            members = UserDAO.ConvertToUserEntities(team.Members)
        };
    }
}