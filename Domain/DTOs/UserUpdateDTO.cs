using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UserUpdateDTO
    {
        public ulong Id { get; }
        public string Password { get; set; }
        public Dictionary<string, Tuple<DateTime, DateTime>> RecurAvailablity;

        public UserUpdateDTO(ulong Id)
        {
            this.Id = Id;
        }
    }
}
