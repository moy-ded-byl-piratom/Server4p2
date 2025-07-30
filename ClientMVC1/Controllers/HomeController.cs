using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientMVC1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            var nickname = User.FindFirst("nickname")?.Value;
            ViewData["Nickname"] = nickname ?? "Guest";
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var client = _httpClientFactory.CreateClient();
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            client.SetBearerToken(accessToken);

            var response = await client.GetAsync("https://localhost:5033/api/user/profile");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ViewData["ProfileData"] = content;
            }

            return View();
        }
    }
}
