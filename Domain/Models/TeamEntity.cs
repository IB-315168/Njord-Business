namespace Domain.Models;


public class TeamEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public MemberEntity TeamLeader { get; set; }
    public ICollection<MemberEntity> members { get; set; }
}