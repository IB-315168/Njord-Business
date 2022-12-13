using Application.Logic;
using Application.LogicInterfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Domain.DTOs.Member;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IAuthService authService;

        public AuthController(IConfiguration config, IAuthService authService)
        {
            this.config = config;
            this.authService = authService;
        }


        /// <summary>
        /// Generates Auth token for valid log in request.
        /// </summary>
        /// <param name="dto">DTO containing email and password of member</param>
        /// <returns>JWT Token generated for a member</returns>
        [HttpPost]
        public async Task<ActionResult> LoginAsync(MemberLoginDTO dto)
        {
            try
            {
                MemberEntity member = await authService.LoginAsync(dto);
                string token = GenerateJwt(member);

                return Ok(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e.Message);
            }
        }

        private List<Claim> GenerateClaims(MemberEntity member)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, config["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.Name, member.UserName),
                new Claim(ClaimTypes.PrimarySid, member.Id.ToString(), ClaimValueTypes.Integer),
                new Claim("Id", member.Id.ToString()),
                new Claim("UserName", member.UserName),
                new Claim("Email", member.Email),
                new Claim("FullName", member.FullName)
            };
            return claims.ToList();
        }

        private string GenerateJwt(MemberEntity member)
        {
            List<Claim> claims = GenerateClaims(member);

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            SigningCredentials signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            JwtHeader header = new JwtHeader(signIn);

            JwtPayload payload = new JwtPayload(
                config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                null,
                DateTime.UtcNow.AddMinutes(60));

            JwtSecurityToken token = new JwtSecurityToken(header, payload);

            string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return serializedToken;
        }
    }
}
