using Application.DAOInterfaces;
using Domain.DTOs;
using Domain.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcNjordClient;
using System.Linq;

namespace Data.DAOs;

public class UserDAO : IUserDAO
{
    private readonly UserService.UserServiceClient userService;

    public UserDAO()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:6565");
        userService = new UserService.UserServiceClient(channel);
    }
    public async Task<UserEntity> CreateAsync(UserEntity user)
    {
        User createdUser = await userService.CreateUserAsync(new CreatingUser()
        {
            UserName = user.UserName,
            FullName = user.FullName,
            Email = user.Email,
            Password = user.Password
        });

        UserEntity userEntity = ConvertToUserEntity(createdUser);
        
        return userEntity;
    }

    public async Task<Task> UpdateAsync(UserEntity user) //with return
    {
        UserEntity? existing = await GetByIdAsync(user.Id);

        if (existing == null)
        {
            throw new Exception($"User with id {user.Id} has not been found");
        }

        await userService.UpdateUserAsync(new UpdatingUser()
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Password = user.Password,
            
        });

        return Task.CompletedTask;
    }

    public async Task<Task> DeleteAsync(UserEntity user)
    {
        await userService.DeleteUserAsync(new Int32Value() { Value = user.Id });

        return Task.CompletedTask;
    }

    public async Task<UserEntity?> GetByEmailAsync(string eMail)
    {
        User? existing = await userService.GetByEmailAsync(new StringValue() { Value = eMail });

        UserEntity userEntity = ConvertToUserEntity(existing);

        return userEntity;
    }

    public async Task<UserEntity?> GetByIdAsync(int id)
    {
        User? existing = await userService.GetByIdAsync(new Int32Value() { Value = id} );

        UserEntity userEntity = ConvertToUserEntity(existing);

        return userEntity;
    }
    public async Task<IEnumerable<UserEntity>> GetByParameterAsync(SearchUserParametersDTO searchParameters)
    {
        string username  = "", email = "", fullname = "";


        if(searchParameters.FullName != null)
        {
            fullname = searchParameters.FullName;
        }

        if(searchParameters.Email != null)
        {
            email = searchParameters.Email;
        }

        if(searchParameters.UserName != null)
        {
            username = searchParameters.UserName;
        }

        UserList users = await userService.SearchUserAsync(new SearchingUser()
        {
            UserName = username,
            Email = email,
            FullName = fullname
        });

        List<UserEntity> result = new List<UserEntity>();

        foreach (User user in users.User)
        {
            result.Add(ConvertToUserEntity(user));
        }

        return result;
    }
    public async Task<UserEntity?> GetByUserNameAsync(string userName)
    {
        UserEntity? existing = new UserEntity();
        return existing;
    }

    public static User ConvertToUser(UserEntity userEntity)
    {
        return new User()
        {
            Id = userEntity.Id,
            UserName = userEntity.UserName,
            Email = userEntity.Email,
            FullName = userEntity.FullName,
            Password = userEntity.Password
        };
    }

    public static ICollection<User> ConvertToUsers(ICollection<UserEntity> users)
    {
        ICollection<User> entities = new List<User>();

        foreach (UserEntity user in users)
        {
            entities.Add(ConvertToUser(user));
        }

        return entities;
    }

    public static UserEntity ConvertToUserEntity(User user)
    {
        return new UserEntity()
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FullName = user.FullName,
            Password = user.Password
        };
    } 

    public static ICollection<UserEntity> ConvertToUserEntities(ICollection<User> users)
    {
        ICollection<UserEntity> entities = new List<UserEntity>();

        foreach(User user in users)
        {
            entities.Add(ConvertToUserEntity(user));
        }

        return entities;
    }

    public async Task<IEnumerable<TeamBasicDTO>> GetUserTeamsAsync(int id)
    {
        User? existing = await userService.GetByIdAsync(new Int32Value() { Value = id });

        List<TeamBasicDTO> teams = new List<TeamBasicDTO>();

        foreach(BasicTeamRet team in existing.UserTeams)
        {
            teams.Add(new TeamBasicDTO(team.Id, team.Name, team.TeamLeaderName));
        }

        return teams;
    }
}