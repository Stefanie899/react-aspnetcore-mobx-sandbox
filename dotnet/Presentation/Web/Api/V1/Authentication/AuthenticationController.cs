using AndcultureCode.CSharp.Core.Enumerations;
using AndcultureCode.CSharp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Sandbox.Presentation.Web.Dtos.Authentication;
using AndcultureCode.CSharp.Core.Interfaces.Conductors;
using Sandbox.Business.Core.Models.Users;
using System.Linq;

namespace Sandbox.Presentation.Web.Api.V1.Authentication
{
    [ApiController]
    [Route("api/v1/authentication")]
    public class AuthenticationController : Controller
    {
        private IRepositoryReadConductor<User> _userReadConductor;

        public AuthenticationController(
            IRepositoryReadConductor<User> userReadConductor
        )
        {
            _userReadConductor = userReadConductor;
        }

        [HttpPost]
        public IActionResult Post([FromBody] AuthenticationRequestDto dto)
        {
            var userResult = _userReadConductor.FindAll(e => e.Username == dto.Username && e.Password == dto.Password);

            if (userResult.ResultObject == null)
            {
                return BadRequest<AuthenticationResponseDto>(null, new Error() { Key = "Credentials Incorrect", Message = "Username or Password is incorrect.", ErrorType = ErrorType.Error});
            }

            var user = userResult.ResultObject.FirstOrDefault();

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

            response.Authenticated = true;
            response.FirstName     = user.FirstName;
            response.LastName      = user.LastName;
            response.Username      = user.Username;
            response.Token         = tokenHandler.WriteToken(token);

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
