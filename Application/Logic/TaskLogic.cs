using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs.Task;
using Domain.Models;

namespace Application.Logic;

public class TaskLogic : ITaskLogic
{
    private readonly ITaskDAO taskDao;
    private readonly IProjectDAO projectDao;
    private readonly IMemberDAO memberDao;

    public TaskLogic(ITaskDAO taskDao, IProjectDAO projectDao, IMemberDAO memberDao)
    {
        this.taskDao = taskDao;
        this.projectDao = projectDao;
        this.memberDao = memberDao;
    }

    public async Task<TaskEntity> CreateAsync(TaskCreateDTO dto)
    {
        ProjectEntity? existing = await projectDao.GetByIdAsync(dto.projectAssigned);

        if (existing == null)
        {
            throw new Exception($"Project with id {dto.projectAssigned} does not exist");
        }

        ValidateTitle(dto.Title);
        ValidateDescription(dto.Description);

        return await taskDao.CreateAsync(dto);
    }

    public async Task UpdateAsync(TaskUpdateDTO dto)
    {
        TaskEntity? existing = await taskDao.GetByIdAsync(dto.Id);

        if (existing == null)
        {
            throw new Exception($"Task with ID {dto.Id} does not exist");
        }

        string title = existing.Title;

        if (!string.IsNullOrEmpty(dto.Title))
        {
            if (!title.Equals(dto.Title))
            {
                ValidateTitle(dto.Title);
            }
        }
        else
        {
            throw new Exception("Title cannot be empty");
        }

        string description = existing.Description;
        if (!string.IsNullOrEmpty(dto.Description))
        {
            if (!description.Equals(dto.Description))
            {
                ValidateDescription(dto.Description);
            }
        }

        string status = dto.Status;
        if (!(status.Equals("Todo") || status.Equals("In progress") || status.Equals("Completed")))
        {
            throw new Exception("Status can only be Todo, In-progress or Completed");
        }

        if(dto.memberAssigned != null)
        {
            MemberEntity? existingMember = await memberDao.GetByIdAsync(dto.memberAssigned.Id);
            if (existingMember == null)
            {
                throw new Exception($"Member with ID {dto.memberAssigned.Id} does not exist");
            }
        }

        DateTime timeEstimation = existing.TimeEstimation;
        if ((dto.TimeEstimation.Hour < 0) || (dto.TimeEstimation.Minute < 0) || (dto.TimeEstimation.Second < 0))
        {
            throw new Exception($"Time estimation cannot be negative value");
        }

        await taskDao.UpdateAsync(dto);
    }

    public async Task DeleteAsync(int id)
    {
        TaskEntity? existing = await taskDao.GetByIdAsync(id);
        if (existing == null)
        {
            throw new Exception($"Task with ID {id} not found!");
        }

        await taskDao.DeleteAsync(id);
    }

    public async Task<TaskEntity> GetByIdAsync(int id)
    {
        TaskEntity? existing = await taskDao.GetByIdAsync(id);
        if (existing == null)
        {
            throw new Exception($"Task with id {id} does not exist.");
        }

        return existing;
    }

    public async Task<ICollection<TaskEntity>> GetByProjectIdAsync(int id)
    {
        ProjectEntity? existing = await projectDao.GetByIdAsync(id);
        if (existing == null)
        {
            throw new Exception($"Project with ID {id} does not exist");
        }

        ICollection<TaskEntity> taskList = await taskDao.GetByProjectIdAsync(id);
        return taskList;
    }
    
    private void ValidateTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new Exception("Title must not be empty");
        }

        if (title.Length < 3 || title.Length > 100)
        {
            throw new Exception("Title must be between 3 and 100 characters.");
        }
    }

    private void ValidateDescription(string description)
    {
        if (description.Length > 500)
        {
            throw new Exception("Description cannot be longer than 500 characters.");
        }
    }
    
}