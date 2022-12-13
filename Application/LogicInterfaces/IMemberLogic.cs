using Domain.DTOs.Member;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.LogicInterfaces
{
    public interface IMemberLogic
    {
        Task<MemberEntity> CreateAsync(MemberCreateDTO dto);
        Task<MemberBasicDTO> GetByIdAsync(int id);
        Task UpdateAsync(MemberUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<MemberEntity>> GetByParameterAsync(SearchMemberParametersDTO searchParameters);
    }
}
