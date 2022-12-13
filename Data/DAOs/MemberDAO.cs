using Application.DAOInterfaces;
using Domain.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcNjordClient;
using System.Linq;
using GrpcNjordClient.Member;
using Domain.DTOs.Member;
using Domain.DTOs.Team;
using Data.Converters;

namespace Data.DAOs;

public class MemberDAO : IMemberDAO
{
    private readonly MemberService.MemberServiceClient memberService;

    public MemberDAO()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:6565");
        memberService = new MemberService.MemberServiceClient(channel);
    }
    public async Task<MemberEntity> CreateAsync(MemberEntity member)
    {
        MemberGrpc createdMember = await memberService.CreateMemberAsync(new CreatingMember()
        {
            UserName = member.UserName,
            FullName = member.FullName,
            Email = member.Email,
            Password = member.Password
        });

        MemberEntity memberEntity = ConvertToMemberEntity(createdMember);
        
        return memberEntity;
    }

    public async Task<Task> UpdateAsync(MemberEntity member) //with return
    {
        MemberEntity? existing = await GetByIdAsync(member.Id);

        if (existing == null)
        {
            throw new Exception($"Member with id {member.Id} has not been found");
        }

        List<MemberAvailabilityGrpc> availabilityGrpcs = new List<MemberAvailabilityGrpc>();
        foreach(AvailabilityEntity availability in member.Availability)
        {
            availabilityGrpcs.Add(new MemberAvailabilityGrpc()
            {
                Id = availability.Id,
                Dayofweek = availability.DayOfWeek,
                Assignedmember = member.Id,
                Starthour = SpecificDateTimeConverter.ConvertToSpecificTime(availability.startHour),
                Endhour = SpecificDateTimeConverter.ConvertToSpecificTime(availability.endHour)
            }) ;
        }


        await memberService.UpdateMemberAsync(new UpdatingMember()
        {
            Id = member.Id,
            UserName = member.UserName,
            Email = member.Email,
            Password = member.Password,
            Availability = {availabilityGrpcs}
        });

        return Task.CompletedTask;
    }

    public async Task<Task> DeleteAsync(MemberEntity member)
    {
        await memberService.DeleteMemberAsync(new Int32Value() { Value = member.Id });

        return Task.CompletedTask;
    }

    public async Task<MemberEntity?> GetByEmailAsync(string eMail)
    {
        MemberGrpc? existing = await memberService.GetByEmailAsync(new StringValue() { Value = eMail });

        MemberEntity memberEntity = ConvertToMemberEntity(existing);

        return memberEntity;
    }

    public async Task<MemberEntity?> GetByIdAsync(int id)
    {
        MemberGrpc? existing = await memberService.GetByIdAsync(new Int32Value() { Value = id} );

        MemberEntity memberEntity = ConvertToMemberEntity(existing);

        Console.WriteLine(memberEntity.Availability.Count());

        return memberEntity;
    }
    public async Task<IEnumerable<MemberEntity>> GetByParameterAsync(SearchMemberParametersDTO searchParameters)
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

        MemberList members = await memberService.SearchMemberAsync(new SearchingMember()
        {
            UserName = username,
            Email = email,
            FullName = fullname
        });

        List<MemberEntity> result = new List<MemberEntity>();

        foreach (MemberGrpc member in members.Member)
        {
            result.Add(ConvertToMemberEntity(member));
        }

        return result;
    }
    public async Task<MemberEntity?> GetByUserNameAsync(string userName)
    {
        MemberEntity? existing = new MemberEntity();
        return existing;
    }

    public static MemberGrpc ConvertToMember(MemberEntity memberEntity)
    {
        return new MemberGrpc()
        {
            Id = memberEntity.Id,
            UserName = memberEntity.UserName,
            Email = memberEntity.Email,
            FullName = memberEntity.FullName,
            Password = memberEntity.Password,
        };
    }

    public static ICollection<MemberGrpc> ConvertToMembers(ICollection<MemberEntity> members)
    {
        ICollection<MemberGrpc> entities = new List<MemberGrpc>();

        foreach (MemberEntity member in members)
        {
            entities.Add(ConvertToMember(member));
        }

        return entities;
    }

    public static MemberEntity ConvertToMemberEntity(MemberGrpc member)
    {

        if (member == null)
        {
            return null;
        } else
        {
            return new MemberEntity()
            {
                Id = member.Id,
                UserName = member.UserName,
                Email = member.Email,
                FullName = member.FullName,
                Password = member.Password,
                Availability = convertAvailability(member)
            };
        }
    } 

    public static List<AvailabilityEntity> convertAvailability(MemberGrpc memberGrpc)
    {
        List<AvailabilityEntity> availabilities = new List<AvailabilityEntity>();

        int currentDayOfWeek = (int)(DateTime.Now.DayOfWeek + 6) % 7;
        DateTime currentDateTime = DateTime.Now.AddDays(-currentDayOfWeek);

        for(int i = 0; i < 7; i++)
        {
            foreach(MemberAvailabilityGrpc memberAvailabilityGrpc in memberGrpc.Availability.Where(aval => aval.Dayofweek == i)) 
            {
                availabilities.Add(new AvailabilityEntity()
                {
                    Id = memberAvailabilityGrpc.Id,
                    DayOfWeek = i,
                    memberName = memberGrpc.UserName,
                    startHour = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, memberAvailabilityGrpc.Starthour.Hour, memberAvailabilityGrpc.Starthour.Minute, 00),
                    endHour = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, memberAvailabilityGrpc.Endhour.Hour, memberAvailabilityGrpc.Endhour.Minute, 00)
                });
            }

            currentDateTime = currentDateTime.AddDays(1);
        }

        return availabilities;
    }

    public static List<MemberAvailabilityGrpc> convertToAvailabilityGrpc(MemberUpdateDTO updateDTO)
    {
        List<MemberAvailabilityGrpc> availabilityGrpcs = new List<MemberAvailabilityGrpc>();

        foreach(AvailabilityEntity entity in updateDTO.Availability)
        {
            availabilityGrpcs.Add(new MemberAvailabilityGrpc()
            {
                Id = entity.Id,
                Assignedmember = updateDTO.Id,
                Dayofweek = entity.DayOfWeek,
                Starthour = SpecificDateTimeConverter.ConvertToSpecificTime(entity.startHour),
                Endhour = SpecificDateTimeConverter.ConvertToSpecificTime(entity.endHour)
            });
        }

        return availabilityGrpcs;
    }

    public static ICollection<MemberEntity> ConvertToMemberEntities(ICollection<MemberGrpc> members)
    {
        ICollection<MemberEntity> entities = new List<MemberEntity>();

        foreach(MemberGrpc member in members)
        {
            entities.Add(ConvertToMemberEntity(member));
        }

        return entities;
    }

    public async Task<IEnumerable<TeamBasicDTO>> GetMemberTeamsAsync(int id)
    {
        MemberGrpc? existing = await memberService.GetByIdAsync(new Int32Value() { Value = id });

        List<TeamBasicDTO> teams = new List<TeamBasicDTO>();

        foreach(BasicTeam team in existing.MemberTeams)
        {
            teams.Add(new TeamBasicDTO(team.Id, team.Name, team.TeamLeaderName));
        }

        return teams;
    }
}