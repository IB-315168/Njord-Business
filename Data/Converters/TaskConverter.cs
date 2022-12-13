using Domain.DTOs.Task;
using Domain.Models;
using GrpcNjordClient.Task;

namespace Data.Converters;

public class TaskConverter
{
    public static TaskEntity ConvertToTaskEntity(TaskGrpc task)
    {
        return new TaskEntity(task.Id, task.Title, task.Description, task.Status,
            SpecificDateTimeConverter.convertToDateTime(task.Creationdate));
    }
}