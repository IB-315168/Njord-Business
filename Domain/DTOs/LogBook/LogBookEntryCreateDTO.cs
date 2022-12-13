namespace Domain.DTOs.LogBook;

public class LogBookEntryCreateDTO
{
    public int idlogbook { get; }
    public int idmeeting { get; }
    public string content { get; }

    public LogBookEntryCreateDTO(int idlogbook, int idmeeting, string content)
    {
        this.idlogbook = idlogbook;
        this.idmeeting = idmeeting;
        this.content = content;
    }
}