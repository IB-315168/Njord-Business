using Application.LogicInterfaces;
using Domain.DTOs.Task;
using Domain.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ITaskLogic taskLogic;

    public TasksController(ITaskLogic taskLogic)
    {
        this.taskLogic = taskLogic;
    }

    /// <summary>
    /// Creates a Task with information from parameters
    /// </summary>
    /// <param name="dto">DTO containing projectAssigned, title, description, status and creation date</param>
    /// <returns>Generated task</returns>
    [HttpPost]
    public async Task<ActionResult<TaskEntity>> CreateAsync(TaskCreateDTO dto)
    {
        try
        {
            TaskEntity task = await taskLogic.CreateAsync(dto);
            return Created($"/tasks/{task.Id}", task);
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
    /// Get task with specific ID
    /// </summary>
    /// <param name="id">Wished Task ID</param>
    /// <returns>Task or nothing in case there is no task with parameter id</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskEntity>> GetByIdAsync([FromRoute] int id)
    {
        try
        {
            return await taskLogic.GetByIdAsync(id);
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
    /// Delete a task with an id from parameters
    /// </summary>
    /// <param name="id">ID of task to be deletetd</param>
    /// <returns>Ok message if id is valid</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await taskLogic.DeleteAsync(id);
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
    /// Updatet a task with information from parameters
    /// </summary>
    /// <param name="dto">DTO containing id, member assigned, title, description, status and time estimation</param>
    /// <returns>Updated task</returns>
    [HttpPatch]
    public async Task<ActionResult> UpdateAsync(TaskUpdateDTO dto)
    {
        try
        {
            await taskLogic.UpdateAsync(dto);
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
    /// Gett list of tasks with specfic project Id
    /// </summary>
    /// <param name="id">ID of Project</param>
    /// <returns>List of tasks whose Project id is the same as the one in the parameters</returns>
    [HttpGet]
    public async Task<ActionResult<ICollection<TaskEntity>>> GetByProjectIdAsync([FromQuery] int id)
    {
        try
        {
            ICollection<TaskEntity> taskList = await taskLogic.GetByProjectIdAsync(id);
            return Ok(taskList);
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