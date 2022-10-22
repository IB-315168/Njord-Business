using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;
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

    [HttpPost]
    public async Task<ActionResult<Team>> CreateAsync(TeamCreateDTO dto)
    {
        try
        {
            Team team = await teamLogic.CreateAsync(dto);
            return Created($"/teams/{team.Id}", team);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Team>> GetByIdAsync([FromRoute] int id)
    {   
        try
        {
            return await teamLogic.GetByIdAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamBasicDTO>>> GetByUserIdAsync([FromQuery] int userId)
    {   
        try
        {
            IEnumerable<TeamBasicDTO> teams = await teamLogic.GetByUserIdAsync(userId);
            return Ok(teams);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}