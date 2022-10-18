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
        ulong userId = 1;
        if (context.Users.Any())
        {
            userId = context.Users.Max(u => u.Id); 
            userId++;
        }
        user.Id = userId;

        string[] name = user.FullName.Split(' ');
        string userName = $"{name[0].ToLower()}_{name[1].Substring(0, 2).ToLower()}#{userId}";
        user.UserName = userName;

        context.Users.Add(user);
        context.SaveChanges();
        
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user) //with return
    {
        User? existing = context.Users.FirstOrDefault(u => u.Id == user.Id);
        if (existing == null)
        {
            throw new Exception($"User with id {user.Id} has not been found");
        }

        context.Users.Remove(existing);
        context.Users.Add(user);
        context.SaveChanges();

        return Task.CompletedTask;
    }

    public async Task DeleteAsync(User user) //asyn
    {
        User? existing = context.Users.FirstOrDefault(u => u.Id == user.Id);

        if(existing == null)
        {
            throw new Exception($"User with id {user.Id} has not been found");
        }

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