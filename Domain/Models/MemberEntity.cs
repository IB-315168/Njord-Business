using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class MemberEntity : IEquatable<MemberEntity>
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public List<AvailabilityEntity> Availability { get; set; }

        public bool Equals(MemberEntity? other)
        {
            if(other == null)
            {
                return false;
            }

            return Id == other.Id &&
                FullName.Equals(other.FullName) &&
                Email.Equals(other.Email) &&
                UserName.Equals(other.UserName);
        }
    }
}
