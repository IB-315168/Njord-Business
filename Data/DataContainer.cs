using Domain.Models;

namespace Data;

public class DataContainer
{
    public ICollection<User> Users { get; set; }
    public ICollection<Team> Teams { get; set; }
}