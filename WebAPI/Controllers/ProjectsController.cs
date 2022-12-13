using Application.LogicInterfaces;
using Domain.DTOs.Project;
using Domain.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectLogic projectLogic;

    public ProjectsController(IProjectLogic projectLogic)
    {
        this.projectLogic = projectLogic;
    }

    /// <summary>
    /// Creates a Project with information from parameters
    /// </summary>
    /// <param name="dto">DTO containing name, team id and deadline </param>
    /// <returns>Generated Project</returns>
    [HttpPost]
    public async Task<ActionResult<ProjectEntity>> CreateAsync(ProjectCreateDTO dto)
    {
        try
        {
            ProjectEntity project = await projectLogic.CreateAsync(dto);
            return Created($"/projects/{project.Id}", project);
        }
        catch (RpcException e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Status.Detail);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Get Project through id sent in parameters
    /// </summary>
    /// <param name="id">Wished Project ID</param>
    /// <returns>Project or nothing in case there is no project with parameter id</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectEntity>> GetByIdAsync([FromRoute] int id)
    {   
        try
        {
            return await projectLogic.GetByIdAsync(id);
        }
        catch (RpcException e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Status.Detail);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Get list of Projects whose X member is part of.
    /// </summary>
    /// <param name="userId">Id of X member</param>
    /// <returns>List of projects whos member with userId is part of</returns>
    [HttpGet]
    public async Task<ActionResult<ICollection<BasicProjectDTO>>> GetByUserIdAsync([FromQuery] int userId)
    {   
        try
        {
            ICollection<BasicProjectDTO> basicProjects = await projectLogic.GetByMemberIdAsync(userId);
            return Ok(basicProjects);
        }
        catch (RpcException e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Status.Detail);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// Delete a Project with id from parameters
    /// </summary>
    /// <param name="id">ID of project to be deleted</param>
    /// <returns>Ok message if id is valid</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await projectLogic.DeleteAsync(id);
            return Ok();
        }
        catch (RpcException e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Status.Detail);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Update a Project with information from parameters
    /// </summary>
    /// <param name="dto">DTO containing id, name, deadline and list of requirements</param>
    /// <returns>Updated project</returns>
    [HttpPatch]
    public async Task<ActionResult> UpdateAsync(ProjectUpdateDTO dto)
    {
        try
        {
            await projectLogic.UpdateAsync(dto);
            return Ok();
        }
        catch (RpcException e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Status.Detail);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}