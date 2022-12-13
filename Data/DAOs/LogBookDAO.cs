using Application.DAOInterfaces;
using Data.Converters;
using Domain.DTOs.LogBook;
using Domain.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcNjordClient.LogBook;

namespace Data.DAOs;

public class LogBookDAO : ILogBookDAO
{
    private readonly LogBookService.LogBookServiceClient logbookService;

    public LogBookDAO()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:6565");
        logbookService = new LogBookService.LogBookServiceClient(channel);
    }
    
    public async Task<LogBookEntity> CreateAsync(LogBookCreateDTO dto)
     {
        LogBookGrpc createdLogBook = await logbookService.CreateLogBookAsync(new CreatingLogBook()
            {
                Projectassigned = dto.ProjectId
            }
        );
        return LogBookConverter.ConvertToLogBookEntity(createdLogBook);
    }

    public async Task<Task> UpdateAsync(LogBookUpdateDTO dto)
    {
        LogBookGrpc updatedLogBook = await logbookService.UpdateLogBookAsync(new UpdatingLogBook()
            {
                Id = dto.Id,
                Logbookentries = { LogBookConverter.convertToEntries(dto.entries) }
            }
        );
        return Task.CompletedTask;
    }

    public async Task<Task> DeleteAsync(int projectId)
    {
        await logbookService.DeleteLogBookByProjectIdAsync(new Int32Value() { Value = projectId });
        return Task.CompletedTask;
    }

    public async Task<LogBookEntity> GetByIdAsync(int id)
    {
        LogBookGrpc reply = await logbookService.GetByIdAsync(new Int32Value() { Value = id });
        LogBookEntity entity = LogBookConverter.ConvertToLogBookEntity(reply);
        return entity;
    }

    public async Task<LogBookEntity?> GetByProjectIdAsync(int id)
    {
        LogBookGrpc reply = await logbookService.GetByProjectIdAsync(new Int32Value() { Value = id });
        LogBookEntity entity = LogBookConverter.ConvertToLogBookEntity(reply);
        return entity;
    }
}