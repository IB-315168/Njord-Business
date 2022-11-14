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

    public async Task<TeamEntity> CreateAsync(TeamCreateDTO dto)
    {
        ValidateName(dto.Name);
        //TeamEntity? eExisting = await teamDao.GetByName(dto.Name);

        //if (eExisting != null)
        //{
        //    throw new Exception("Name already in use");
        //}

        UserEntity? existing = await userDao.GetByIdAsync(dto.TeamLeaderId);

        if (existing == null)
        {
            throw new Exception($"User with id {dto.TeamLeaderId} does not exist");
        }

        TeamEntity toCreate = new TeamEntity
        {
            Name = dto.Name,
            TeamLeader = existing,
            members = new List<UserEntity>()
        };

        TeamEntity created = await teamDao.CreateAsync(toCreate);

        return created;
    }

    public async Task<TeamEntity> GetByIdAsync(int id)
    {
        TeamEntity? existing = await teamDao.GetByIdAsync(id);
        
        if(existing == null)
        {
            throw new Exception($"Team with id {id} does not exist.");
        }

        return existing;
    }

    public async Task<IEnumerable<TeamBasicDTO>> GetByUserIdAsync(int id)
    {
        UserEntity? existing = await userDao.GetByIdAsync(id);

        if (existing == null)
        {
            throw new Exception($"User with id {id} does not exist.");
        }

        IEnumerable<TeamBasicDTO> teams = await userDao.GetUserTeamsAsync(id);

        return teams;
    }

    public async Task DeleteAsync(int id)
    {
        TeamEntity? existing = await teamDao.GetByIdAsync(id);
        if (existing == null)
        {
            throw new Exception($"Team with ID {id} not found!");
        }

        await teamDao.DeleteAsync(existing);
    }

    public async Task UpdateAsync(TeamUpdateDTO dto)
    {
        TeamEntity? existing = await teamDao.GetByIdAsync(dto.Id);
        
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
        UserEntity teamLeader = existing.TeamLeader;
        //check if new teamleader is part of a team
        if (dto.members.Contains(teamLeader))
        {
            if (!teamLeader.Equals(dto.TeamLeader))
            {
                ValidateTeamLeader(dto.TeamLeader);
                teamLeader = dto.TeamLeader;
            }
        }

        ICollection<UserEntity> members = existing.members;
        if (!members.Equals(dto.members))
        {
            members = dto.members;
        }
        TeamEntity updated = new TeamEntity()
        {
            Id = dto.Id,
            members = dto.members,
            Name = dto.Name,
            TeamLeader = dto.TeamLeader
        };
        await teamDao.UpdateAsync(updated);
    }
    private void ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new Exception("Name must not be empty");
        }
        
        if (name.Length < 5 || name.Length > 20)
        {
            throw new Exception("Name must be between 5 and 20 characters.");
        }
    }

    private void ValidateTeamLeader(UserEntity teamLeader)
    {
        if (string.IsNullOrEmpty(teamLeader.ToString()))
        {
            throw new Exception("Team Leader must not be empty");
        }
    }
}



