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
        public async Task<User> CreateAsync(UserCreationDTO dto)
        {
            ValidateData(dto);

            User? eExisting = await userDAO.GetByEmailAsync(dto.Email);

            if (eExisting != null)
            {
                throw new Exception("E-mail address already in use");
            }

            User? uExisting = await userDAO.GetByUserNameAsync(dto.UserName);

            if (uExisting != null)
            {
                throw new Exception("Username already in use");
            }


            User toCreate = new User
            {
                UserName = dto.UserName,
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
            if (!string.IsNullOrEmpty(dto.Password))
            {
                if (!password.Equals(dto.Password))
                {
                    ValidatePassword(dto.Password);
                    password = dto.Password;
                }
            }

            string email = existing.Email;
            if(!string.IsNullOrEmpty(dto.Email))
            {
                if(!email.Equals(dto.Email))
                {
                    ValidateEmail(dto.Email);

                    User? eExisting = await userDAO.GetByEmailAsync(dto.Email);

                    if (eExisting != null)
                    {
                        throw new Exception("E-mail address already in use");
                    }
                    email = dto.Email;
                }
            }

            string username = existing.UserName;
            if(!string.IsNullOrEmpty(dto.UserName))
            {
                if(!username.Equals(dto.UserName))
                {
                    ValidateUsername(dto.UserName);
                    User? uExisting = await userDAO.GetByUserNameAsync(dto.UserName);

                    if (uExisting != null)
                    {
                        throw new Exception("Username already in use");
                    }
                    username = dto.UserName;
                }
            }

            // TODO: Implement recurring availability validation
            Dictionary<string, bool[]> availability = dto.RecurAvailablity;

            User user = new User()
            {
                Id = dto.Id,
                FullName = existing.FullName,
                Email = email,
                UserName = username,
                Password = password,
                RecurAvailablity = availability
            };

            await userDAO.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            User? existing = await userDAO.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception($"User with ID {id} not found!");
            }

            await userDAO.DeleteAsync(existing);
        }

        private void ValidateData(UserCreationDTO dto)
        {
            Regex fullNameVal = new Regex(@"(^[A-Za-z]{2,16})([ ])([A-Za-z]{2,16})");

            if (string.IsNullOrEmpty(dto.FullName))
            {
                throw new Exception("Full name must not be empty");
            }

            if (!fullNameVal.IsMatch(dto.FullName))
            {
                throw new Exception("Full name: \n- should consist only of latin alphabet letters (A-Z, a-z)\n- should not contain any special characters (!,@,#,$,...) or digits\n- should be in format \"FirstName LastName\"");
            }

            ValidateEmail(dto.Email);
            ValidatePassword(dto.Password);
            ValidateUsername(dto.UserName);
        }

        public static void ValidateEmail(string email)
        {
            Regex emailVal = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Email must not be empty");
            }

            if (!emailVal.IsMatch(email))
            {
                throw new Exception("Please input correct email.");
            }
        }

        private void ValidatePassword(string Password)
        {
            if (string.IsNullOrEmpty(Password))
            {
                throw new Exception("Password must not be empty");
            }

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

        private void ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new Exception("Username must not be empty");
            }

            if (username.Length < 5 || username.Length > 20)
            {
                throw new Exception("Username must be between 5 and 20 characters.");
            }
        }

        public async Task<UserBasicDTO> GetByIdAsync(int id)
        {
            User? existing = await userDAO.GetByIdAsync(id);

            if(existing == null)
            {
                throw new Exception($"User with id {id} does not exist.");
            }

            UserBasicDTO userBasic = new UserBasicDTO(existing.Id, existing.FullName, existing.Email, existing.UserName, existing.RecurAvailablity);

            return userBasic;
        }
        public async Task<IEnumerable<User>> GetByParameterAsync(SearchUserParametersDTO searchParameters)
        {
            return await userDAO.GetByParameterAsync(searchParameters);
        }
    }
}
