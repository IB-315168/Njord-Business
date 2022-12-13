using Domain.Models;

namespace Domain.DTOs.Team;

public class TeamBasicDTO
{
    public int Id { get; }
    public string Name { get; }
    public string TeamLeaderName { get; }

    public TeamBasicDTO(int id, string name, string teamLeaderName)
    {
        Id = id;
        Name = name;
        TeamLeaderName = teamLeaderName;
    }
}