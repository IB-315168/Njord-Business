using Application.LogicInterfaces;
using Domain.DTOs.Team;
using Domain.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TeamsController :ControllerBase
{
    private readonly ITeamLogic teamLogic;

    public TeamsController(ITeamLogic teamLogic)
    {
        this.teamLogic = teamLogic;
    }

    /// <summary>
    /// Creates a team with information from parameters
    /// </summary>
    /// <param name="dto">DTO containing name and team leader id</param>
    /// <returns>Generated team</returns>
    [HttpPost]
    public async Task<ActionResult<TeamEntity>> CreateAsync(TeamCreateDTO dto)
    {
        try
        {
            TeamEntity team = await teamLogic.CreateAsync(dto);
            return Created($"/teams/{team.Id}", team);
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
    /// Get a Team with specific ID
    /// </summary>
    /// <param name="id">Wished Team ID</param>
    /// <returns>Team or nothing in case there is no team with parameter id</returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TeamEntity>> GetByIdAsync([FromRoute] int id)
    {   
        try
        {
            return await teamLogic.GetByIdAsync(id);
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
    /// Get list of teams whose a specific member is part of
    /// </summary>
    /// <param name="userId">Member id </param>
    /// <returns>List of teams which member with same id as userId is part of.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamBasicDTO>>> GetByUserIdAsync([FromQuery] int userId)
    {   
        try
        {
            IEnumerable<TeamBasicDTO> teams = await teamLogic.GetByMemberIdAsync(userId);
            return Ok(teams);
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
    /// Delete a team with an id from parameters
    /// </summary>
    /// <param name="id">ID of team to be deleted</param>
    /// <returns>Ok message if id is valid</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await teamLogic.DeleteAsync(id);
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
    /// Update a team with information from parameters
    /// </summary>
    /// <param name="dto">DTO containing id, name, team leader and list of members</param>
    /// <returns>Updated team</returns>
    [HttpPatch]
    public async Task<ActionResult> UpdateAsync(TeamUpdateDTO dto)
    {
        try
        {
            await teamLogic.UpdateAsync(dto);
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