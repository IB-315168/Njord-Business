using Application.DAOInterfaces;
using Domain.Models;

namespace Data.DAOs;

public class UserDAO : IUserDAO
{
    private readonly FileContext context;

    public UserDAO(FileContext context)
    {
        this.context = context;
    }
    public Task<User> CreateAsync(User user)
    {
        int userId = 1;
        if (context.Users.Any())
        {
            userId = (int)context.Users.Max(u => u.Id); //cast
            userId++;
        }
        user.Id = (ulong)userId; // cast
        
        context.Users.Add(user);
        context.SaveChanges();
        
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user) //with return
    {
        User? existing = context.Users.FirstOrDefault(u => u.Id == user.Id);
        existing.FullName = user.FullName;
        existing.Email = user.Email;
        existing.UserName = user.UserName;
        existing.Password = user.Password;
        return Task.FromResult(existing);
    }

    public async Task DeleteAsync(User user) // async
    {
        User? existing = context.Users.FirstOrDefault(u => u.Id == user.Id);
        context.Users.Remove(existing);
    }

    public Task<User?> GetByEmailAsync(string eMail)
    {
        User? existing = context.Users.FirstOrDefault(u => u.Email == eMail);
        return Task.FromResult(existing);
    }

    public Task<User?> GetByIdAsync(ulong id)
    {
        User? existing = context.Users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(existing);
    }
}