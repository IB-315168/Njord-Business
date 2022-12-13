using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs.LogBook;
using Domain.Models;

namespace Application.Logic;

public class LogBookLogic : ILogBookLogic
{
    private readonly ILogBookDAO logbookDao;
    private readonly IProjectDAO projectDao;

    public LogBookLogic(ILogBookDAO logbookDao, IProjectDAO projectDao)
    {
        this.logbookDao = logbookDao;
        this.projectDao = projectDao;
    }
    
    public async Task<LogBookEntity> CreateAsync(LogBookCreateDTO dto)
    {
        ProjectEntity? existing = await projectDao.GetByIdAsync(dto.ProjectId);
        if (existing == null)
        {
            throw new Exception($"Project with id {dto.ProjectId} does not exist");
        }
        return await logbookDao.CreateAsync(dto);
    }

    public async Task UpdateAsync(LogBookUpdateDTO dto)
    {
        LogBookEntity? existing = await logbookDao.GetByIdAsync(dto.Id);
        if (existing == null)
        {
            throw new Exception($"Logbook with id {dto.Id} does not exist");
        }

        await logbookDao.UpdateAsync(dto);
    }

    public async Task DeleteAsync(int id)
    {
        LogBookEntity? existing = await logbookDao.GetByIdAsync(id);
        if (existing == null)
        {
            throw new Exception($"Logbook with id {id} does not exist");
        }
        await logbookDao.DeleteAsync(id);
    }

    public async Task<LogBookEntity?> GetByIdAsync(int id)
    {
        LogBookEntity? existing = await logbookDao.GetByIdAsync(id);
        
        if(existing == null)
        {
            throw new Exception($"Logbook with id {id} does not exist.");
        }

        return existing;
    }

    public async Task<LogBookEntity?> GetByProjectIdAsync(int id)
    {
        LogBookEntity? existing = await logbookDao.GetByProjectIdAsync(id);
        
        if (existing == null)
        {
            throw new Exception($"Logbook assigned to project ID {id} does not exist.");
        }

        return existing;
    }
}