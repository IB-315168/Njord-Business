using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.LogicInterfaces
{
    public interface IUserLogic
    {
        Task<User> CreateAsync(UserCreationDTO dto);
        Task<User> LoginAsync(UserLoginDTO dto);
        Task UpdateAsync(UserUpdateDTO dto);
        Task DeleteAsync(int id);
    }
}
