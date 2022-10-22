using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class TeamsController :ControllerBase
{
    private readonly ITeamLogic teamLogic;

    public TeamsController(ITeamLogic teamLogic)
    {
        this.teamLogic = teamLogic;
    }

    [HttpPost]
    [ActionName("CreateAsync")]
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
    [ActionName("GetByIdAsync")]
    public async Task<ActionResult<TeamBasicDTO>> GetByIdAsync([FromRoute] int id)
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
    [HttpGet("{id:int}")]
    [ActionName("GetByUserIdAsync")]
    public async Task<ActionResult<TeamBasicDTO>> GetByUserIdAsync([FromRoute]int id)
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