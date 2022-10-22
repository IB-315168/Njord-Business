namespace Domain.Models;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; }
    public User TeamLeader { get; set; }
    public ICollection<User> members { get; set; }


}