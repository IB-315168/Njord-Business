using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs.Member;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Logic
{
    public class MemberLogic : IMemberLogic
    {
        private readonly IMemberDAO memberDAO;

        public MemberLogic(IMemberDAO memberDAO)  
        {
            this.memberDAO = memberDAO;
        }
        public async Task<MemberEntity> CreateAsync(MemberCreateDTO dto)
        {
            ValidateData(dto);

            MemberEntity toCreate = new MemberEntity
            {
                UserName = dto.UserName,
                FullName = dto.FullName,
                Email = dto.Email,
                Password = dto.Password,
            };

            MemberEntity created = await memberDAO.CreateAsync(toCreate);

            return created;
        }

        public async Task UpdateAsync(MemberUpdateDTO dto)
        {
            MemberEntity? existing = await memberDAO.GetByIdAsync(dto.Id);

            if (existing == null)
            {
                throw new Exception($"Member with ID {dto.Id} not found!");
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
                    email = dto.Email;
                }
            }

            string username = existing.UserName;
            if(!string.IsNullOrEmpty(dto.UserName))
            {
                if(!username.Equals(dto.UserName))
                {
                    ValidateUsername(dto.UserName);
                    username = dto.UserName;
                }
            }

            List<AvailabilityEntity> availability = dto.Availability;

            MemberEntity member = new MemberEntity()
            {
                Id = dto.Id,
                FullName = existing.FullName,
                Email = email,
                UserName = username,
                Password = password,
                Availability = availability
            };

            await memberDAO.UpdateAsync(member);
        }

        public async Task DeleteAsync(int id)
        {
            MemberEntity? existing = await memberDAO.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception($"Member with ID {id} not found!");
            }

            await memberDAO.DeleteAsync(existing);
        }

        private void ValidateData(MemberCreateDTO dto)
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

        public async Task<MemberBasicDTO> GetByIdAsync(int id)
        {
            MemberEntity? existing = await memberDAO.GetByIdAsync(id);

            if(existing == null)
            {
                throw new Exception($"Member with id {id} does not exist.");
            }

            MemberBasicDTO memberBasic = new MemberBasicDTO(existing.Id, existing.FullName, existing.Email, existing.UserName) { Availability = existing.Availability};

            return memberBasic;
        }
        public async Task<IEnumerable<MemberEntity>> GetByParameterAsync(SearchMemberParametersDTO searchParameters)
        {
            return await memberDAO.GetByParameterAsync(searchParameters);
        }
    }
}
