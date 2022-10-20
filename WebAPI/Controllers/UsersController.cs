﻿using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserLogic userLogic;

        public UsersController(IUserLogic userLogic)
        {
            this.userLogic = userLogic;
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateAsync(UserCreationDTO dto)
        {
            try
            {
                User user = await userLogic.CreateAsync(dto);
                return Created($"/users/{user.Id}", user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserBasicDTO>> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                return await userLogic.GetByIdAsync(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<User>> LoginAsync(UserLoginDTO dto)
        {
            try
            {
                return await userLogic.LoginAsync(dto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateAsync(UserUpdateDTO dto)
        {
            try
            {
                await userLogic.UpdateAsync(dto);
                return Ok();
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
                await userLogic.DeleteAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}
