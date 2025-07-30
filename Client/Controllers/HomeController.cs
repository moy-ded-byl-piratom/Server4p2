using Client.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace Client.Controllers
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
            var email = User.FindFirst("email")?.Value;
            ViewData["Email"] = email ?? "Guest";
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var client = _httpClientFactory.CreateClient();
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            client.SetBearerToken(accessToken);

            var response = await client.GetAsync("http://localhost:7117/api/values");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ViewData["ProfileData"] = content;
            }
            else
            {
                ViewData["ProfileData"] = $"Error: {response.StatusCode}";
            }

            return View();
        }
        //public async Task<IActionResult> Index()
        //{
        //    var accessToken = await HttpContext.GetTokenAsync("access_token");

        //    var client = new HttpClient();
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //    var response = await client.GetAsync("https://localhost:5001/api/values");

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return Content($"API Error: {response.StatusCode}");
        //    }

        //    var content = await response.Content.ReadAsStringAsync();

        //    return Content($"API Response: {content}");
        //}
    }
}
