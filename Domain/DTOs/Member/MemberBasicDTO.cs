using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Member
{
    public class MemberBasicDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public List<AvailabilityEntity> Availability { get; set; }

        public MemberBasicDTO(int id, string fullName, string email, string userName)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            UserName = userName;
        }
    }
}
