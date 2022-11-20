using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<UserEntity>> CreateAsync(UserCreateDTO dto)
        {
            try
            {
                UserEntity user = await userLogic.CreateAsync(dto);
                return Created($"/users/{user.Id}", user);
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
        public async Task<ActionResult<UserBasicDTO>> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                return await userLogic.GetByIdAsync(id);
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
        public async Task<ActionResult> UpdateAsync(UserUpdateDTO dto)
        {
            try
            {
                await userLogic.UpdateAsync(dto);
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

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                await userLogic.DeleteAsync(id);
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserEntity>>> GetAsync([FromQuery] string? username,string? fullname,string? email)
        {
            try
            {
                SearchUserParametersDTO parameters = new(username, email, fullname);
                IEnumerable<UserEntity> users = await userLogic.GetByParameterAsync(parameters);
                return Ok(users);
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
}
