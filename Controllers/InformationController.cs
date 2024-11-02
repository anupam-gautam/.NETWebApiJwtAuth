using JwtWebApiAuth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtWebApiAuth.Controllers
{
    [Route("api/Information")]
    [ApiController]
    public class InformationController : ControllerBase
    {
        [HttpGet("User")]
        [Authorize]
        public IActionResult GetUserInformation()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi {currentUser.FirstName}, you are on {currentUser.Role} role");
        }

        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {
            return Ok("Access to admin page granted");
        }

        [HttpGet("Editor")]
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult EditorPage()
        {
            return Ok("Access to editor page granted");
        }

        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new UserModel
                {
                    UserName = userClaims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                    EmailAddress = userClaims.First(x => x.Type == ClaimTypes.Email).Value,
                    FirstName = userClaims.First(x => x.Type == ClaimTypes.GivenName).Value,
                    LastName = userClaims.First(x => x.Type == ClaimTypes.Surname).Value,
                    Role = userClaims.First(x => x.Type == ClaimTypes.Role).Value,
                };
            }
            return null;
        }
    }
}
