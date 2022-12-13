using Application.DAOInterfaces;
using Data.Converters;
using Domain.DTOs.Meeting;
using Domain.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcNjordClient.Meeting;

namespace Data.DAOs;

public class MeetingDAO : IMeetingDAO
{
    private readonly MeetingService.MeetingServiceClient meetingService;
    
    public MeetingDAO()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:6565");
        meetingService = new MeetingService.MeetingServiceClient(channel);
    }
    
    public async Task<MeetingEntity> CreateAsync(MeetingCreateDTO dto)
    {
        MeetingGrpc createdMeeting = await meetingService.CreateMeetingAsync(new CreatingMeeting()
        {
            Assignedleader = dto.AssignedLeader,
            ProjectAssigned = dto.ProjectAssigned,
            Title = dto.Title,
            Description = dto.Description,
            Startdate = SpecificDateTimeConverter.convertToSpecificDateTime(dto.StartDate),
            Enddate = SpecificDateTimeConverter.convertToSpecificDateTime(dto.EndDate)
        });
        MeetingEntity meetingEntity = ConvertToMeetingEntity(createdMeeting);
        return meetingEntity;
    }

    public async Task<Task> UpdateAsync(MeetingUpdateDTO dto)
    {
        await meetingService.UpdateMeetingAsync(new UpdatingMeeting()
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            Startdate = SpecificDateTimeConverter.convertToSpecificDateTime(dto.StartDate),
            Enddate = SpecificDateTimeConverter.convertToSpecificDateTime(dto.EndDate)
        });
        return Task.CompletedTask;
    }

    public async Task<Task> DeleteAsync(int id)
    {
        await meetingService.DeleteMeetingAsync(new Int32Value() { Value = id });
        return Task.CompletedTask;
    }

    public async Task<MeetingEntity?> GetByIdAsync(int id)
    {
        MeetingEntity? existing = ConvertToMeetingEntity(await meetingService.GetByIdAsync(new Int32Value() { Value = id }));
        return existing;
    }

    public async Task<ICollection<MeetingEntity>> GetByProjectIdAsync(int id)
    {
        GrpcMeetingList meetingList = await meetingService.GetByProjectIdAsync(new Int32Value() {Value = id});

        List<MeetingEntity> result = new List<MeetingEntity>();

        foreach (MeetingGrpc meeting in meetingList.MeetingList)
        {
            result.Add(ConvertToMeetingEntity(meeting));
        }

        return result;
    }

    public static MeetingGrpc ConvertToMeetingGrpc(MeetingEntity meetingEntity)
    {
        return new MeetingGrpc()
        {
            Id = meetingEntity.Id,
            Assignedleader = MemberDAO.ConvertToMember(meetingEntity.AssignedLeader),
            Title = meetingEntity.Title,
            Description = meetingEntity.Description,
            Startdate = SpecificDateTimeConverter.convertToSpecificDateTime(meetingEntity.StartDate),
            Enddate = SpecificDateTimeConverter.convertToSpecificDateTime(meetingEntity.EndDate)
        };
    }

    public static MeetingEntity ConvertToMeetingEntity(MeetingGrpc meeting)
    {
        return new MeetingEntity(meeting.Id, MemberDAO.ConvertToMemberEntity(meeting.Assignedleader), meeting.Title, meeting.Description,
            SpecificDateTimeConverter.convertToDateTime(meeting.Startdate), SpecificDateTimeConverter.convertToDateTime(meeting.Enddate));
    }

    public static BasicMeetingDTO ConvertToBasicMeetingDTO(BasicMeeting meeting)
    {
        return new BasicMeetingDTO(meeting.Id, meeting.Title,
            SpecificDateTimeConverter.convertToDateTime(meeting.Startdate),
            SpecificDateTimeConverter.convertToDateTime(meeting.Enddate));
    }
}