using AndcultureCode.CSharp.Core.Enumerations;
using AndcultureCode.CSharp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web.Dtos.Authentication;

namespace Web.Api.V1.Authentication
{
    [ApiController]
    [Route("api/v1/authentication")]
    public class AuthenticationController : Controller
    {
        [HttpPost]
        public IActionResult Post([FromBody] AuthenticationRequestDto dto)
        {
            if (dto.Username.ToLower() != "test" || dto.Password.ToLower() != "test")
            {
                return BadRequest<AuthenticationResponseDto>(null, new Error() { Key = "Credentials Incorrect", Message = "Username or Password is incorrect.", ErrorType = ErrorType.Error});
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("some_big_key_value_here_secret");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "1")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var response = new AuthenticationResponseDto();

            response.Authenticated = (dto.Username.ToLower() == "test" && dto.Password.ToLower() == "test");
            response.FirstName     = "Test";
            response.LastName      = "LastName";
            response.Username      = "test";
            response.Token         = tokenHandler.WriteToken(token);

            if (!response.Authenticated)
            {
                return Ok(response, new Error() { Key = "Credentials Incorrect", Message = "Username or Password is incorrect.", ErrorType = ErrorType.Error});
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Testing the thing.");
        }
    }
}
