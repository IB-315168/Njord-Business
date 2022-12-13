using Application.LogicInterfaces;
using Domain.DTOs.LogBook;
using Domain.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogBookController : ControllerBase
{
    private readonly ILogBookLogic logbookLogic;

    public LogBookController(ILogBookLogic logbookLogic)
    {
        this.logbookLogic = logbookLogic;
    }
    
    /// <summary>
    /// Creates a Logbook with information from parameters
    /// </summary>
    /// <param name="dto">DTO containing id project</param>
    /// <returns>Generated logbook</returns>
    [HttpPost]
    public async Task<ActionResult<LogBookEntity>> CreateAsync(LogBookCreateDTO dto)
    {
        try
        {
            LogBookEntity logbook = await logbookLogic.CreateAsync(dto);
            return Created($"/logbooks/{logbook.Id}", logbook);
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
    /// Get a Logbook with an id from parameters
    /// </summary>
    /// <param name="id">Wished logbook ID</param>
    /// <returns>Logbook or Nothing in case there is no logbook with parameter id</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<LogBookEntity>> GetByIdAsync([FromRoute] int id)
    {
        try
        {
            return await logbookLogic.GetByIdAsync(id);
        }
        catch (RpcException e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// Remove a Logbook with id from parameters
    /// </summary>
    /// <param name="id">ID of Logbook to be removed</param>
    /// <returns> Ok message if ID was valid</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await logbookLogic.DeleteAsync(id);
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
    /// Update a Logbook with information from parameters
    /// </summary>
    /// <param name="dto">DTO containing id and list of entries</param>
    /// <returns>Updated logbook</returns>
    [HttpPatch]
    public async Task<ActionResult> UpdateAsync(LogBookUpdateDTO dto)
    {
        try
        {
            await logbookLogic.UpdateAsync(dto);
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
    /// Get a Logbook with Project id from parameters
    /// </summary>
    /// <param name="id">Wished Logbook with Project ID</param>
    /// <returns>Logbook or nothing in case there is no logbook with project id parameter</returns>
    [HttpGet]
    public async Task<ActionResult<LogBookEntity?>> GetByProjectIdAsync([FromQuery] int id)
    {
        try
        {
            LogBookEntity? logBookEntity = await logbookLogic.GetByProjectIdAsync(id);
            return Ok(logBookEntity);
        }
        catch (RpcException e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

}