namespace Domain.DTOs;

public class UserDeleteDTO
{
    public ulong Id { get; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}