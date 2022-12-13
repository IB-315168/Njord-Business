using Domain.Models;

namespace Domain.DTOs.Team;

public class TeamUpdateDTO
{
    public int Id { get; }
    public string Name { get; set; }
    public MemberEntity TeamLeader { get; set; }
    public ICollection<MemberEntity> members { get; set; }

    public TeamUpdateDTO(int id)
    {
        Id = id;
    }
}