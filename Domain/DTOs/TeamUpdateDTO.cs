using Domain.Models;

namespace Domain.DTOs;

public class TeamUpdateDTO
{
    public int Id { get;  }
    public string Name { get; set; }
    public UserEntity TeamLeader { get; set; }
    public ICollection<UserEntity> members { get; set; }

    public TeamUpdateDTO(int id)
    {
        this.Id = id;
    }
}