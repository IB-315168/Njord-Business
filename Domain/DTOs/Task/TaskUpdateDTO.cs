using Domain.Models;

namespace Domain.DTOs.Task;

public class TaskUpdateDTO
{
    public int Id { get; }
    public MemberEntity? memberAssigned { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime TimeEstimation { get; set; }

    public TaskUpdateDTO(int id)
    {
        Id = id;
    }
}