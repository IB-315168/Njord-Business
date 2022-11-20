﻿using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class TeamsController :ControllerBase
{
    private readonly ITeamLogic teamLogic;

    public TeamsController(ITeamLogic teamLogic)
    {
        this.teamLogic = teamLogic;
    }

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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamBasicDTO>>> GetByUserIdAsync([FromQuery] int userId)
    {   
        try
        {
            IEnumerable<TeamBasicDTO> teams = await teamLogic.GetByUserIdAsync(userId);
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