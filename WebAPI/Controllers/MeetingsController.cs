using Application.LogicInterfaces;
using Domain.DTOs.Meeting;
using Domain.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MeetingsController : ControllerBase
{
    private readonly IMeetingLogic meetingLogic;

    public MeetingsController(IMeetingLogic meetingLogic)
    {
        this.meetingLogic = meetingLogic;
    }
    
    /// <summary>
    /// Creates a Meeting with information from parameters
    /// </summary>
    /// <param name="dto">DTO containing id of Leader, id of Project, title, description, start date and end date</param>
    /// <returns>Generated meeting</returns>
    [HttpPost]
    public async Task<ActionResult<MeetingEntity>> CreateAsync(MeetingCreateDTO dto)
    {
        try
        {
            MeetingEntity meeting = await meetingLogic.CreateAsync(dto);
            return Created($"/meetings/{meeting.Id}", meeting);
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
    /// Get a Meeting with an id from parameters
    /// </summary>
    /// <param name="id">Wished meeting ID</param>
    /// <returns>Meeting or nothing in case there is no meeting with parameter id</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MeetingEntity>> GetByIdAsync([FromRoute] int id)
    {   
        try
        {
            return await meetingLogic.GetByIdAsync(id);
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
    /// Delete a Meeting with an id from parameters
    /// </summary>
    /// <param name="id">ID of Meeting to be removed </param>
    /// <returns> Ok message if ID was valid </returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await meetingLogic.DeleteAsync(id);
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
    /// Update a Meeting with information from parameters
    /// </summary>
    /// <param name="dto">DTO containing id , title, description, start date and end date</param>
    /// <returns>Updated meetitng</returns>
    [HttpPatch]
    public async Task<ActionResult> UpdateAsync(MeetingUpdateDTO dto)
    {
        try
        {
            await meetingLogic.UpdateAsync(dto);
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
    /// Get a list of meetings with project id
    /// </summary>
    /// <param name="id">ID of project</param>
    /// <returns>List of meetings whose project id is the same as the one sent in parameters</returns>
    [HttpGet]
    public async Task<ActionResult<ICollection<MeetingEntity>>> GetByProjectIdAsync([FromQuery] int id)
    {   
        try
        {
            ICollection<MeetingEntity> meetingList=await meetingLogic.GetByProjectIdAsync(id);
            return Ok(meetingList);
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