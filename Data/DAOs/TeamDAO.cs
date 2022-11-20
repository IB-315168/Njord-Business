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
        await teamService.DeleteTeamAsync(new Int32Value() { Value = team.Id });

        return Task.CompletedTask;
    }
    
    public async Task<Task> UpdateAsync(TeamEntity team)
    {
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

        TeamEntity entity = ConvertToTeamEntity(reply);

        return entity;
    }

    public async Task<TeamEntity?> GetByIdAsync(int id)
    {
        Team? reply = await teamService.GetByIdAsync(new Int32Value { Value = id });

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