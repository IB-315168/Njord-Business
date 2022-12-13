using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs.Member;
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
        private readonly IMemberDAO memberDAO;

        public AuthService(IMemberDAO memberDAO)
        {
            this.memberDAO = memberDAO;
        }
        public async Task<MemberEntity> LoginAsync(MemberLoginDTO dto)
        {
            MemberLogic.ValidateEmail(dto.Email);

            MemberEntity? existing = await memberDAO.GetByEmailAsync(dto.Email);

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
