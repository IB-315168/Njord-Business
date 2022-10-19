using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DAOInterfaces
{
    public interface IUserDAO
    {
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User?> GetByEmailAsync(string eMail);
        Task<User?> GetByUserNameAsync(string userName);
        Task<User?> GetByIdAsync(int id);
    }
}
