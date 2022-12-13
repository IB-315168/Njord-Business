using Application.DAOInterfaces;
using Domain.DTOs;
using Domain.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcNjordClient.Team;
using System.Xml.Linq;
using GrpcNjordClient.Member;

namespace Data.DAOs;

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
        TeamGrpc createdTeam = await teamService.CreateTeamAsync(new CreatingTeam() { Name = team.Name, TeamLeaderId = team.TeamLeader.Id });


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
            TeamLeader = MemberDAO.ConvertToMember(team.TeamLeader),
            Members = {MemberDAO.ConvertToMembers(team.members)}
        });

        return Task.CompletedTask;
    }
    public async Task<TeamEntity?> GetByName(string name)
    {
        TeamGrpc? reply = await teamService.GetByNameAsync(new StringValue() { Value = name });

        TeamEntity entity = ConvertToTeamEntity(reply);

        return entity;
    }

    public async Task<TeamEntity?> GetByIdAsync(int id)
    {
        TeamGrpc? reply = await teamService.GetByIdAsync(new Int32Value { Value = id });

        TeamEntity entity = ConvertToTeamEntity(reply);

        return entity;
    }

    public static TeamEntity ConvertToTeamEntity(TeamGrpc team)
    {
        return new TeamEntity()
        {
            Id = team.Id,
            Name = team.Name,
            TeamLeader = MemberDAO.ConvertToMemberEntity(team.TeamLeader),
            members = MemberDAO.ConvertToMemberEntities(team.Members)
        };
    }
}