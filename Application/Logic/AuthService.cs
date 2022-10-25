using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Logic
{
    public class AuthService : IAuthService
    {
        private readonly IUserDAO userDAO;

        public AuthService(IUserDAO userDAO)
        {
            this.userDAO = userDAO;
        }
        public async Task<User> LoginAsync(UserLoginDTO dto)
        {
            UserLogic.ValidateEmail(dto.Email);

            User? existing = await userDAO.GetByEmailAsync(dto.Email);

            if (existing == null)
            {
                throw new Exception("Account with that email does not exist.");
            }

            if (!dto.Password.Equals(existing.Password))
            {
                throw new Exception("Password incorrect.");
            }

            return existing;
        }
    }
}
