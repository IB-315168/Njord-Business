using System.Collections;
using System.Text.RegularExpressions;
using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace Application.Logic;

public class TeamLogic : ITeamLogic
{
    private readonly ITeamDAO teamDao;
    private readonly IUserDAO userDao;
    
    public TeamLogic(ITeamDAO teamDAO,IUserDAO userDao)
    {
        this.teamDao = teamDAO;
        this.userDao = userDao;
    }

    public async Task<Team> CreateAsync(TeamCreateDTO dto)
    {   
        //ValidateData(dto);
        Team? eExisting = await teamDao.GetByName(dto.Name);
        // to be discussed
        if (eExisting != null)
        {
            throw new Exception("Name already in use");
        }

        User? existing = await userDao.GetByIdAsync(dto.TeamLeaderId);

        if (existing == null)
        {
            throw new Exception($"User with id {dto.TeamLeaderId} does not exist");
        }

        Team toCreate = new Team
        {
            Name = dto.Name,
            TeamLeader = existing,
            members = new List<User>()
        };

        Team created = await teamDao.CreateAsync(toCreate);

        return created;
    }

    public async Task<Team> GetByIdAsync(int id)
    {
        Team? existing = await teamDao.GetByIdAsync(id);
        
        if(existing == null)
        {
            throw new Exception($"Team with id {id} does not exist.");
        }

        return existing;
    }

    public async Task<IEnumerable<TeamBasicDTO>> GetByUserIdAsync(int id)
    {
        User? existing = await userDao.GetByIdAsync(id);

        if (existing == null)
        {
            throw new Exception($"User with id {id} does not exist.");
        }

        List<TeamBasicDTO> teams = new List<TeamBasicDTO>();
        IEnumerable<Team> teamsFetched = await teamDao.GetByUserIdAsync(id);

        foreach(Team team in teamsFetched)
        {
            teams.Add(new TeamBasicDTO(team.Id, team.Name, $"{team.TeamLeader.FullName} ({team.TeamLeader.UserName})"));
        }

        return teams;
    }


    private void ValidateData(TeamCreateDTO dto)
    {
        Regex NameVal = new Regex(@"(^[A-Za-z]{2,16})");

        if (string.IsNullOrEmpty(dto.Name))
        {
            throw new Exception("Name must not be empty");
        }
        if (!NameVal.IsMatch(dto.Name))
        {
            throw new Exception("Name: \n- should consist only of latin alphabet letters (A-Z, a-z)\n- should not contain any special characters (!,@,#,$,...) or digits");
        }
    }
}