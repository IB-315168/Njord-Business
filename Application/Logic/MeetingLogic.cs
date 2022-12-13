using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs.Meeting;
using Domain.Models;

namespace Application.Logic;

public class MeetingLogic : IMeetingLogic
{
    private readonly IMeetingDAO meetingDao;
    private readonly IMemberDAO memberDao;
    private readonly IProjectDAO projectDao;
    public MeetingLogic(IMeetingDAO meetingDao, IMemberDAO memberDao, IProjectDAO projectDao)
    {
        this.meetingDao = meetingDao;
        this.memberDao = memberDao;
        this.projectDao = projectDao;
    }

    public async Task<MeetingEntity> CreateAsync(MeetingCreateDTO dto)
    {
        MemberEntity? existing = await memberDao.GetByIdAsync(dto.AssignedLeader);
        if (existing == null)
        {
            throw new Exception($"Member with ID {dto.AssignedLeader} does not exist.");
        }

        ProjectEntity? projectEntity = await projectDao.GetByIdAsync(dto.ProjectAssigned);
        if (projectEntity == null)
        {
            throw new Exception($"Project with ID {dto.ProjectAssigned} does not exist.");
        }
        
        ValidateTitle(dto.Title);
        ValidateDescription(dto.Description);
        ValidateDates(dto.StartDate, dto.EndDate);
        
        return await meetingDao.CreateAsync(dto);
    }

    public async Task UpdateAsync(MeetingUpdateDTO dto)
    {
        MeetingEntity? existing = await meetingDao.GetByIdAsync(dto.Id);
        if (existing == null)
        {
            throw new Exception($"Meeting with ID {dto.Id} does not exist");
        }
        
        string title = existing.Title;
        if (!string.IsNullOrEmpty(dto.Title))
        {
            if (!title.Equals(dto.Title))
            {
                ValidateTitle(dto.Title);
            }
        } else
        {
            throw new Exception("Meeting title cannot be empty");
        }
        
        string description = existing.Description;
        if (!string.IsNullOrEmpty(dto.Description))
        {
            if (!description.Equals(dto.Description))
            {
                ValidateDescription(dto.Description);
            }
        }

        DateTime startDate = existing.StartDate;
        DateTime endDate = existing.EndDate;
        if (!dto.StartDate.Equals(null) || !dto.EndDate.Equals(null))
        {
            if (!startDate.Equals(dto.StartDate) || !endDate.Equals(dto.EndDate))
            {
                ValidateDates(dto.StartDate, dto.EndDate);
            }
        } else
        {
            throw new Exception("Meeting must have a start and end date");
        }

        await meetingDao.UpdateAsync(dto);
    }

    public async Task DeleteAsync(int id)
    {
        MeetingEntity? existing = await meetingDao.GetByIdAsync(id);
        if (existing == null)
        {
            throw new Exception($"Meeting with ID {id} does not exist");
        }

        await meetingDao.DeleteAsync(id);
    }

    public async Task<MeetingEntity?> GetByIdAsync(int id)
    {
        MeetingEntity? existing = await meetingDao.GetByIdAsync(id);
        if (existing == null)
        {
            throw new Exception($"Meeting with ID {id} does not exist");
        }
        return existing;
    }

    public async Task<ICollection<MeetingEntity>> GetByProjectIdAsync(int id)
    {
        ProjectEntity? existing = await projectDao.GetByIdAsync(id);
        if (existing == null)
        {
            throw new Exception($"Project with ID {id} does not exist");
        }
        ICollection<MeetingEntity> meetingList = await meetingDao.GetByProjectIdAsync(id);
        return meetingList;
    }

    private void ValidateTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new Exception("Title must not be empty.");
        }

        if (title.Length < 3 || title.Length > 20)
        {
            throw new Exception("Title must be between 3 and 20 characters.");
        }
    }
    private void ValidateDescription(string description)
    {
        if (description.Length > 250)
        {
            throw new Exception("Description cannot be longer than 250 characters.");
        }
    }

    private void ValidateDates(DateTime startDate, DateTime endDate)
    {
        if (startDate.Equals(null) || endDate.Equals(null))
        {
            throw new Exception("Dates cannot be null.");
        }
        if (startDate > endDate)
        {
            throw new Exception("Start date cannot be later than End date.");
        }
    }
}