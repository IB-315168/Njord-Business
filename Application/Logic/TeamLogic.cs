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

    public async Task DeleteAsync(int id)
    {
        Team? existing = await teamDao.GetByIdAsync(id);
        if (existing == null)
        {
            throw new Exception($"Team with ID {id} not found!");
        }

        await teamDao.DeleteAsync(existing);
    }

    public async Task UpdateAsync(TeamUpdateDTO dto)
    {
        Team? existing = await teamDao.GetByIdAsync(dto.Id);
        
        if (existing == null)
        {
            throw new Exception($"Team with ID {dto.Id} not found!");
        }
        
        string name = existing.Name;
        if (!string.IsNullOrEmpty(dto.Name))
        {
            if (!name.Equals(dto.Name))
            {
                ValidateName(dto.Name);
                name = dto.Name;
            }   
        }
        User teamLeader = existing.TeamLeader;
        //check if new teamleader is part of a team
        if (dto.members.Contains(teamLeader))
        {
            if (!teamLeader.Equals(dto.TeamLeader))
            {
                ValidateTeamLeader(dto.TeamLeader);
                teamLeader = dto.TeamLeader;
            }
        }

        ICollection<User> members = existing.members;
        if (!members.Equals(dto.members))
        {
            members = dto.members;
        }
        Team updated = new Team()
        {
            Id = dto.Id,
            members = dto.members,
            Name = dto.Name,
            TeamLeader = dto.TeamLeader
        };
        await teamDao.UpdateAsync(updated);
    }
    private async void ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new Exception("Name must not be empty");
        }
        //optional
        if (name.Length < 5 || name.Length > 20)
        {
            throw new Exception("Name must be between 5 and 20 characters.");
        }
    }
    //to string again
    private async void ValidateTeamLeader(User teamLeader)
    {
        if (string.IsNullOrEmpty(teamLeader.ToString()))
        {
            throw new Exception("teamLeader must not be empty");
        }
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



