using Domain.DTOs.Meeting;
using Domain.Models;

namespace Application.DAOInterfaces;

public interface IMeetingDAO
{
    Task<MeetingEntity> CreateAsync(MeetingCreateDTO dto);
    Task<Task> UpdateAsync(MeetingUpdateDTO dto);
    Task<Task> DeleteAsync(int id);
    Task<MeetingEntity?> GetByIdAsync(int id);
    Task<ICollection<MeetingEntity>> GetByProjectIdAsync(int id);
}