using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ProjectEntity : IEquatable<ProjectEntity>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TeamEntity Team { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public ICollection<RequirementEntity> Requirements { get; set; }


        public ProjectEntity(int id, string name, TeamEntity teamId, DateTime startDate)
        {
            Id = id;
            Name = name;
            Team = teamId;
            StartDate = startDate;
        }

        public bool Equals(ProjectEntity? other)
        {
            if(other == null)
            {
                return false;
            }

            return Id == other.Id && Name == other.Name && Team == other.Team;
        }
    }
}
