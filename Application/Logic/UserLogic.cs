using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Logic
{
    public class UserLogic : IUserLogic
    {
        private readonly IUserDAO userDAO;

        public UserLogic(IUserDAO userDAO)
        {
            this.userDAO = userDAO;
        }
        //UNDONE: Awaiting DAO implementation
        public async Task<User> CreateAsync(UserCreationDTO dto)
        {
            // TODO: Decide on which Getter method should be used for retrieveing user
            User? existing = await userDAO.GetByEmailAsync(dto.Email);

            if (existing != null)
                throw new Exception("E-mail address already in use");

            ValidateData(dto);
            // TODO: Assign Id and generate UserName in DAO
            User toCreate = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = dto.Password,
            };

            User created = await userDAO.CreateAsync(toCreate);

            return created;
        }

        private void ValidateData(UserCreationDTO dto)
        {
            Regex emailVal = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            if (string.IsNullOrEmpty(dto.Email))
            {
                throw new Exception("Email must not be empty");
            }

            if (string.IsNullOrEmpty(dto.FullName))
            {
                throw new Exception("Full name must not be empty");
            }

            if (string.IsNullOrEmpty(dto.Password))
            {
                throw new Exception("Password must not be empty");
            }

            if (emailVal.IsMatch(dto.Email)) {
                throw new Exception("Please input correct email.");
            }

            if(!dto.Password.Any(ch => ! Char.IsLetterOrDigit(ch)))
            {
                throw new Exception("Password must contain at least one special character.");
            }

            if(!dto.Password.Any(ch => Char.IsDigit(ch)))
            {
                throw new Exception("Password must contain at least one digit.");
            }

            if (!dto.Password.Any(ch => Char.IsUpper(ch)))
            {
                throw new Exception("Password must contain at least one upper case character.");
            }
        }
    }
}
