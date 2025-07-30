using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            var email = User.FindFirst("email")?.Value;
            
            return Ok(new { email });
        }
    }
}
