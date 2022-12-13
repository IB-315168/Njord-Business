using Application.DAOInterfaces;
using Data.Converters;
using Domain.DTOs.Project;
using Domain.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using GrpcNjordClient.Project;

namespace Data.DAOs;

public class ProjectDAO : IProjectDAO
{
    private readonly ProjectService.ProjectServiceClient projectService;

    public ProjectDAO()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:6565");
        this.projectService = new ProjectService.ProjectServiceClient(channel);
    }

    public async Task<ProjectEntity> CreateAsync(ProjectCreateDTO dto)
    {
        ProjectGrpc createdProject = await projectService.CreateProjectAsync(new CreatingProject()
        {
            Name = dto.Name,
            TeamId = dto.TeamId,
            Deadline = SpecificDateTimeConverter.convertToSpecificDateTime(dto.Deadline)
        });
        return ProjectConverter.ConvertToProjectEntity(createdProject);
    }

    public async Task<Task> UpdateAsync(ProjectUpdateDTO dto)
    {
        await projectService.UpdateProjectAsync(new UpdatingProject()
        {
            Name = dto.Name,
            Id = dto.Id,
            Deadline = SpecificDateTimeConverter.convertToSpecificDateTime(dto.deadline),
            Requirements = { ProjectConverter.convertToRequirements(dto.requirements) }
        });
        return Task.CompletedTask;
    }

    public async Task<Task> DeleteAsync(int id)
    { 
       await projectService.DeleteProjectAsync(new Int32Value(){Value = id});
       return Task.CompletedTask;
    }

    public async Task<ProjectEntity> GetByIdAsync(int id)
    {
        ProjectGrpc reply = await projectService.GetByIdAsync(new Int32Value() { Value = id });
        ProjectEntity entity = ProjectConverter.ConvertToProjectEntity(reply);
        return entity;
    }

    public async Task<ICollection<BasicProjectDTO>> GetByMemberIdAsync(int id)
    {
        BasicProjectList reply = await projectService.GetByMemberIdAsync(new Int32Value() { Value = id });
        ICollection<BasicProjectDTO> result = ProjectConverter.ConvertToCollection(reply);
        return result;
    }
}