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
            throw new Exception("User with id does not exist");
        }
        Team toCreate = new Team
        {
            Name= dto.Name,
            TeamLeaderId =dto.TeamLeaderId,
            members = new List<User>()
        };

        Team created = await teamDao.CreateAsync(toCreate);

        return created;
    }

    public async Task<TeamBasicDTO> GetByIdAsync(int id)
    {
        Team? existing = await teamDao.GetByIdAsync(id);
        
        if(existing == null)
        {
            throw new Exception($"Team with id {id} does not exist.");
        }
        TeamBasicDTO teamBasic = new TeamBasicDTO(existing.Id, existing.Name, existing.TeamLeaderId, existing.members);

        return teamBasic;
    }

    public async Task<TeamBasicDTO> GetByUserIdAsync(int id)
    {
        Team? existing = await teamDao.GetByUserIdAsync(id);
        
        if(existing == null)
        {
            throw new Exception($"User with id {id} does not exist.");
        }
        TeamBasicDTO teamBasic = new TeamBasicDTO(existing.Id, existing.Name, existing.TeamLeaderId, existing.members);

        return teamBasic;
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