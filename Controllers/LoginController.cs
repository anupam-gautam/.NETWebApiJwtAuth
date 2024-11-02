using JwtWebApiAuth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtWebApiAuth.Controllers
{
    [Route("api/Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public dynamic Login([FromBody] UserLogin input)
        {
            var user = Authenticate(input);
            if (user == null)
            {
                return NotFound();
            }
            var token = Generate(user);
            return Ok(token);
        }

        private string Generate(UserModel input)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, input.UserName),
                new Claim(ClaimTypes.Email,input.EmailAddress),
                new Claim(ClaimTypes.GivenName, input.FirstName),
                new Claim(ClaimTypes.Surname, input.LastName),
                new Claim(ClaimTypes.Role, input.Role)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], 
                            _config["Jwt:Audience"],
                            claims,
                            expires: DateTime.Now.AddMinutes(15),
                            signingCredentials: credentials);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenticate(UserLogin input)
        {
            var currentUser = UserConstants.Users
                               .FirstOrDefault(x =>
                                   x.UserName.ToLower() == input.UserName.ToLower() &&
                                   x.Password == input.Password
                               );
            return currentUser == null ? null : currentUser;

        }
    }
}
