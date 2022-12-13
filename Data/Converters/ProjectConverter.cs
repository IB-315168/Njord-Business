using System.Collections;
using Data.DAOs;
using Domain.DTOs.Project;
using Domain.Models;
using GrpcNjordClient.Project;

namespace Data.Converters;

public class ProjectConverter
{
    public static ProjectEntity ConvertToProjectEntity(ProjectGrpc project)
    {
        List<RequirementEntity> requirements = new List<RequirementEntity>();
        foreach(Requirement requirement in project.Requirements)
        {
            requirements.Add(new RequirementEntity(requirement.Id, requirement.Idproject, requirement.Content));
        }

        return new ProjectEntity(project.Id, project.Name, TeamDAO.ConvertToTeamEntity(project.TeamId),
            SpecificDateTimeConverter.convertToDateTime(project.Deadline))
        {
            StartDate = SpecificDateTimeConverter.convertToDateTime(project.StartDate),
            Requirements = requirements
        };
    }
    public static ICollection<BasicProjectDTO> ConvertToCollection(BasicProjectList basicProjectList)
    {
        ICollection<BasicProjectDTO> collection = new List<BasicProjectDTO>();
        foreach (BasicProject basicProject in basicProjectList.Projects) 
        {
            collection.Add(new BasicProjectDTO(basicProject.Id, basicProject.ProjectName, basicProject.TeamName));
        }

        return collection;
    }

    public static Requirement convertToRequirement(RequirementEntity requirementEntity)
    {
        return new Requirement()
        {
            Id = requirementEntity.Id,
            Idproject = requirementEntity.IdProject,
            Content = requirementEntity.content
        };
    }

    public static ICollection<Requirement> convertToRequirements(ICollection<RequirementEntity> requirementEntities)
    {
        ICollection<Requirement> requirements = new List<Requirement>();

        foreach(RequirementEntity entity in requirementEntities)
        {
            requirements.Add(ProjectConverter.convertToRequirement(entity));
        }

        return requirements;
    }
}   