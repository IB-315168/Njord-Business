using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.LogicInterfaces
{
    public interface IUserLogic
    {
        Task<UserEntity> CreateAsync(UserCreateDTO dto);
        Task<UserBasicDTO> GetByIdAsync(int id);
        Task UpdateAsync(UserUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<UserEntity>> GetByParameterAsync(SearchUserParametersDTO searchParameters);
    }
}
