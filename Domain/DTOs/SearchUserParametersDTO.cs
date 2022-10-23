using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class SearchUserParametersDTO
    {
        public string? UserName { get; }
        public string? Email { get; }
        public string? FullName { get; }

        public SearchUserParametersDTO(string? userName, string? email, string? fullName)
        {
            UserName = userName;
            Email = email;
            FullName = fullName;
        }
    }
}
