using Application.LogicInterfaces;
using Domain.DTOs.Member;
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
    public class MembersController : ControllerBase
    {
        private readonly IMemberLogic memberLogic;

        public MembersController(IMemberLogic memberLogic)
        {
            this.memberLogic = memberLogic;
        }

        /// <summary>
        /// Creates a Member with information from parameters
        /// </summary>
        /// <param name="dto">DTO containing FullName, UserName, email, password</param>
        /// <returns>Generated member</returns>
        [HttpPost]
        public async Task<ActionResult<MemberEntity>> CreateAsync(MemberCreateDTO dto)
        {
            try
            {
                MemberEntity member = await memberLogic.CreateAsync(dto);
                return Created($"/members/{member.Id}", member);
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
        /// Get member with ID from parameter
        /// </summary>
        /// <param name="id">Wished Member ID</param>
        /// <returns>Member or nothing in case there is no member with parametetr id</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberBasicDTO>> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                return await memberLogic.GetByIdAsync(id);
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
        /// Update a Member with information from parameters
        /// </summary>
        /// <param name="dto">DTO containing id, username, email, password and list of availability</param>
        /// <returns>Updated member</returns>
        [HttpPatch]
        public async Task<ActionResult> UpdateAsync(MemberUpdateDTO dto)
        {
            try
            {
                await memberLogic.UpdateAsync(dto);
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
        /// Removes a Member with information from parameters
        /// </summary>
        /// <param name="id">ID of member to be removed</param>
        /// <returns> Ok message if ID is valid </returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                await memberLogic.DeleteAsync(id);
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
        /// Get member through filter
        /// </summary>
        /// <param name="username">Optional | Characters that are part of X member usernames.</param>
        /// <param name="fullname">Optional | Characters that are part of X member full name.</param>
        /// <param name="email">Optional | Characters that are part of X member email.</param>
        /// <returns>List of Members who fit the filters</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberEntity>>> GetAsync([FromQuery] string? username,string? fullname,string? email)
        {
            try
            {
                SearchMemberParametersDTO parameters = new(username, email, fullname);
                IEnumerable<MemberEntity> members = await memberLogic.GetByParameterAsync(parameters);
                return Ok(members);
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
