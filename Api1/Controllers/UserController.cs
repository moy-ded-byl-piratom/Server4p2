using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var nickname = User.FindFirst("nickname")?.Value;
            return Ok(new { Nickname = nickname });
        }

    }
}
