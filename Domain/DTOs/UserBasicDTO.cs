using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UserBasicDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, Tuple<DateTime, DateTime>> RecurAvailablity { get; set; }

        public UserBasicDTO(int id, string fullName, string email, string userName, Dictionary<string, Tuple<DateTime, DateTime>> recurAvailablity)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            UserName = userName;
            RecurAvailablity = recurAvailablity;
        }
    }
}
