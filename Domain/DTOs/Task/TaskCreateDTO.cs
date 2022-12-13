using Domain.Models;

namespace Domain.DTOs.Task;

public class TaskCreateDTO
{
    public int projectAssigned { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status = "Todo";
    public DateTime CreationDate = DateTime.Now;
}