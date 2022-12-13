using Application.DAOInterfaces;
using Data.Converters;
using Domain.DTOs.Task;
using Domain.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcNjordClient.Task;

namespace Data.DAOs;

public class TaskDAO : ITaskDAO
{
    private readonly TaskService.TaskServiceClient taskService;

    public TaskDAO()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:6565");
        taskService = new TaskService.TaskServiceClient(channel);
    }


    public async Task<TaskEntity> CreateAsync(TaskCreateDTO dto)
    {
        TaskGrpc createdTask = await taskService.CreateTaskAsync(new CreatingTask()
        {
            ProjectAssigned = dto.projectAssigned,
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            Creationdate = SpecificDateTimeConverter.convertToSpecificDateTime(dto.CreationDate)
            
        });
        TaskEntity taskEntity = ConvertTaskEntity(createdTask);
        return taskEntity;
    }

    public async Task<Task> UpdateAsync(TaskUpdateDTO dto)
    {
        if(dto.memberAssigned == null)
        {
            await taskService.UpdateTaskAsync(new UpdatingTask()
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                Timeestimation = SpecificDateTimeConverter.ConvertToSpecificTime(dto.TimeEstimation),
                Memberassigned = 0
            });
        } else
        {
            await taskService.UpdateTaskAsync(new UpdatingTask()
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                Timeestimation = SpecificDateTimeConverter.ConvertToSpecificTime(dto.TimeEstimation),
                Memberassigned = dto.memberAssigned.Id
            });
        }
        return Task.CompletedTask;
    }

    public async Task<Task> DeleteAsync(int id)
    {
        await taskService.DeleteTaskAsync(new Int32Value() { Value = id });
        return Task.CompletedTask;
    }

    public async Task<TaskEntity> GetByIdAsync(int id)
    {
        TaskGrpc reply = await taskService.GetByIdAsync(new Int32Value() { Value = id });
        TaskEntity entity = TaskConverter.ConvertToTaskEntity(reply);
        return entity;
    }

    public async Task<ICollection<TaskEntity>> GetByProjectIdAsync(int id)
    {
        GrpcTaskList taskList = await taskService.GetByProjectIdAsync(new Int32Value() { Value = id });
        List<TaskEntity> result = new List<TaskEntity>();
        foreach (TaskGrpc tasks in taskList.TasksList)
        {
            result.Add(ConvertTaskEntity(tasks));
        }

        return result;
    }

    public static TaskGrpc ConvertToTaskGrpc(TaskEntity taskEntity)
    {
        return new TaskGrpc()
        {
            Id = taskEntity.Id,
            Title = taskEntity.Title,
            Description = taskEntity.Description,
            Status = taskEntity.Status,
            Creationdate = SpecificDateTimeConverter.convertToSpecificDateTime(taskEntity.CreationDate),
            Timeestimation = SpecificDateTimeConverter.ConvertToSpecificTime(taskEntity.TimeEstimation)
        };
    }

    public static TaskEntity ConvertTaskEntity(TaskGrpc task)
    {
        return new TaskEntity(task.Id, task.Title, task.Description, task.Status, SpecificDateTimeConverter.convertToDateTime(task.Creationdate))
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            CreationDate = SpecificDateTimeConverter.convertToDateTime(task.Creationdate),
            TimeEstimation = SpecificDateTimeConverter.ConvertToTime(task.Timeestimation),
            memberAssigned = MemberDAO.ConvertToMemberEntity(task.Memberassigned)
        };
    }

    public static BasicTaskDTO ConvertToBasicTaskDTO(BasicTask task)
    {
        return new BasicTaskDTO(task.Id, task.Title);
    }
}