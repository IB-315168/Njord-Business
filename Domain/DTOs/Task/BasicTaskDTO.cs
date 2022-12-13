namespace Domain.DTOs.Task;

public class BasicTaskDTO
{
    public int Id { get; }
    public string Title { get; }

    public BasicTaskDTO(int id, string title)
    {
        Id = id;
        Title = title;
    }
}