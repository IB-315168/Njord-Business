using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.LogicInterfaces
{
    public interface IAuthService
    {
        Task<User> LoginAsync(UserLoginDTO dto);
    }
}
