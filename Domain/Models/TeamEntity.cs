namespace Domain.Models;


public class TeamEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public UserEntity TeamLeader { get; set; }
    public ICollection<UserEntity> members { get; set; }
}