using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs.LogBook;
using Domain.DTOs.Project;
using Domain.Models;

namespace Application.Logic;

public class ProjectLogic : IProjectLogic
{
    private readonly IProjectDAO projectDao;
    private readonly ITeamDAO teamDao;
    private readonly ILogBookDAO logbookDao;

    public ProjectLogic(IProjectDAO projectDao, ITeamDAO teamDao, ILogBookDAO logbookDao)
    {
        this.projectDao = projectDao;
        this.teamDao = teamDao;
        this.logbookDao = logbookDao;
    }
    
    public async Task<ProjectEntity> CreateAsync(ProjectCreateDTO dto)
    {
        ValidateName(dto.Name);
        TeamEntity? existing = await teamDao.GetByIdAsync(dto.TeamId);
        if (existing == null)
        {
            throw new Exception($"Team with ID {dto.TeamId} does not exist");
        }

        ProjectEntity projectCreated = await projectDao.CreateAsync(dto);

        await logbookDao.CreateAsync(new LogBookCreateDTO()
        {
            ProjectId = projectCreated.Id
        });

        return projectCreated;
    }

    public async Task DeleteAsync(int id)
    {
        ProjectEntity? existing = await projectDao.GetByIdAsync(id);
        if (existing == null)
        {
            throw new Exception($"Project with ID {id} not found!");
        }

        await logbookDao.DeleteAsync(existing.Id);

        await projectDao.DeleteAsync(id);
    }

    public async Task UpdateAsync(ProjectUpdateDTO dto)
    {
        ProjectEntity? existing = await projectDao.GetByIdAsync(dto.Id);

        if (existing == null)
        {
            throw new Exception($"Project with ID {dto.Id} does not exist");
        }

        string name = existing.Name;
        if (!string.IsNullOrEmpty(dto.Name))
        {
            if (!name.Equals(dto.Name))
            {
                ValidateName(dto.Name);
            }   
        } else
        {
            throw new Exception("Name of the project cannot be empty");
        }

        DateTime startDate = existing.StartDate;
        if (dto.deadline < DateTime.Now)
        {
            throw new Exception($"Deadline cannot be before Start Date");
        }

        await projectDao.UpdateAsync(dto);
    }

    public async Task<ProjectEntity> GetByIdAsync(int id)
    {
        ProjectEntity? existing = await projectDao.GetByIdAsync(id);
        
        if(existing == null)
        {
            throw new Exception($"Project with id {id} does not exist.");
        }

        return existing;
    }

    public async Task<ICollection<BasicProjectDTO>> GetByMemberIdAsync(int id)
    {
        ICollection<BasicProjectDTO> collection = new List<BasicProjectDTO>();
        collection = await projectDao.GetByMemberIdAsync(id);
        return collection;
    }

    private void ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new Exception("Name must not be empty");
        }
        
        else if (name.Length < 5 && name.Length!=0 || name.Length > 25 && name.Length!=0)
        {
            throw new Exception("Name must be between 5 and 25 characters.");
        }
    }
    private void ValidateTeam(TeamEntity teamAssigned)
    {
        if (string.IsNullOrEmpty(teamAssigned.ToString()))
        {
            throw new Exception("Team assgined must not be empty");
        }
    }
}