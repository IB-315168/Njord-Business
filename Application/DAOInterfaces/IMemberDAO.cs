using Domain.DTOs.Member;
using Domain.DTOs.Team;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DAOInterfaces
{
    public interface IMemberDAO
    {
        Task<MemberEntity> CreateAsync(MemberEntity user);
        Task<Task> UpdateAsync(MemberEntity user);
        Task<Task> DeleteAsync(MemberEntity user);
        Task<MemberEntity?> GetByEmailAsync(string eMail);
        Task<MemberEntity?> GetByUserNameAsync(string userName);
        Task<MemberEntity?> GetByIdAsync(int id);
        Task<IEnumerable<MemberEntity>> GetByParameterAsync(SearchMemberParametersDTO dto);
        Task<IEnumerable<TeamBasicDTO>> GetMemberTeamsAsync(int id);
    }
}
