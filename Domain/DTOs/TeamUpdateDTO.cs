using Domain.Models;

namespace Domain.DTOs;

public class TeamUpdateDTO
{
    public int Id { get;  }
    public string Name { get; set; }
    public User TeamLeader { get; set; }
    public ICollection<User> members { get; set; }

    public TeamUpdateDTO(int id)
    {
        this.Id = id;
    }
}