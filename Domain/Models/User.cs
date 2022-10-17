using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        public ulong Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, Tuple<DateTime, DateTime>> RecurAvailablity;
    }
}
