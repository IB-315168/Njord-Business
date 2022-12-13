using Domain.Models;

namespace Domain.DTOs.Team;

public class TeamCreateDTO
{

    public string Name { get; }
    public int TeamLeaderId { get; }

    public TeamCreateDTO(string name, int teamLeaderId)
    {
        Name = name;
        TeamLeaderId = teamLeaderId;
    }
}