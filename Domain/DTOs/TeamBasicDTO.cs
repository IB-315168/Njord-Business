using Domain.Models;

namespace Domain.DTOs;

public class TeamBasicDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TeamLeaderId { get; set; }
    public ICollection<User> members { get; set; }

    public TeamBasicDTO(int id, string name, int teamLeaderId, ICollection<User> members)
    {
        Id = id;
        Name = name;
        TeamLeaderId = teamLeaderId;
        this.members = members;
    }
}