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

        public async Task UpdateAsync(UserUpdateDTO dto)
        {
            User? existing = await userDAO.GetByIdAsync(dto.Id);

            if (existing == null)
            {
                throw new Exception($"User with ID {dto.Id} not found!");
            }

            string password = existing.Password;

            if(!string.IsNullOrEmpty(dto.Password))
            {
                ValidatePassword(dto.Password);
                if (existing.Password.Equals(dto.Password))
                {
                    throw new Exception("New password cannot be the same as your old one.");
                }

                password = dto.Password;
            }

            // TODO: Implement recurring availability validation
            Dictionary<string, Tuple<DateTime, DateTime>> availability = dto.RecurAvailablity ?? existing.RecurAvailablity;

            User user = new User()
            {
                Id = dto.Id,
                FullName = existing.FullName,
                Email = existing.Email,
                UserName = existing.UserName,
                Password = password,
                RecurAvailablity = availability
            };
             
            await userDAO.UpdateAsync(user);
        }

        public async Task DeleteAsync(UserDeleteDTO dto)
        {
            User? existing = await userDAO.GetByIdAsync(dto.Id);
            if (existing == null)
            {
                throw new Exception($"User with ID {dto.Id} not found!");
            }

            await userDAO.DeleteAsync(existing);
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


            if (emailVal.IsMatch(dto.Email)) {
                throw new Exception("Please input correct email.");
            }

            if (string.IsNullOrEmpty(dto.Password))
            {
                throw new Exception("Password must not be empty");
            }

            ValidatePassword(dto.Password);
        }

        private void ValidatePassword(string Password)
        {
            if (!Password.Any(ch => !Char.IsLetterOrDigit(ch)))
            {
                throw new Exception("Password must contain at least one special character.");
            }

            if (!Password.Any(ch => Char.IsDigit(ch)))
            {
                throw new Exception("Password must contain at least one digit.");
            }

            if (!Password.Any(ch => Char.IsUpper(ch)))
            {
                throw new Exception("Password must contain at least one upper case character.");
            }
        }
    }
}
