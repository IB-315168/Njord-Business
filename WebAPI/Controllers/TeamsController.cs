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

    [HttpGet("{userId:int}")]
    public async Task<ActionResult<TeamBasicDTO>> GetByIdAsync([FromRoute] int userId)
    {   
        try
        {
            return await teamLogic.GetByIdAsync(userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<TeamBasicDTO>> GetByUserIdAsync([FromQuery]int id)
    {   
        try
        {
            return await teamLogic.GetByUserIdAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}