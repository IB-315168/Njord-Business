using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DAOInterfaces
{
    public interface IUserDAO
    {
        Task<UserEntity> CreateAsync(UserEntity user);
        Task<Task> UpdateAsync(UserEntity user);
        Task<Task> DeleteAsync(UserEntity user);
        Task<UserEntity?> GetByEmailAsync(string eMail);
        Task<UserEntity?> GetByUserNameAsync(string userName);
        Task<UserEntity?> GetByIdAsync(int id);
        Task<IEnumerable<UserEntity>> GetByParameterAsync(SearchUserParametersDTO dto);
        Task<IEnumerable<TeamBasicDTO>> GetUserTeamsAsync(int id);
    }
}
