using Domain.DTOs.Meeting;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface IMeetingLogic
{
    Task<MeetingEntity> CreateAsync(MeetingCreateDTO dto);
    Task UpdateAsync(MeetingUpdateDTO dto);
    Task DeleteAsync(int id);
    Task<MeetingEntity?> GetByIdAsync(int id);
    Task<ICollection<MeetingEntity>> GetByProjectIdAsync(int id);
}