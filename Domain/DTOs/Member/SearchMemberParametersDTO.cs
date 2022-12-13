using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Member
{
    public class SearchMemberParametersDTO
    {
        public string? UserName { get; }
        public string? Email { get; }
        public string? FullName { get; }

        public SearchMemberParametersDTO(string? userName, string? email, string? fullName)
        {
            UserName = userName;
            Email = email;
            FullName = fullName;
        }
    }
}
