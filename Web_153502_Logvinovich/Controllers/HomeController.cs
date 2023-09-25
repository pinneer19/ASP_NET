using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using System.Net.Http;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Web_153502_Logvinovich.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;

        public HomeController(HttpClient httpClient)
        {
            _client = httpClient;
        }
     
        IEnumerable<ListDemo> list = new List<ListDemo>()
            {
                new ListDemo { Id = 1, Name = "Object 1" },
                new ListDemo { Id = 2, Name = "Object 2" },
                new ListDemo { Id = 3, Name = "Object 3" },
                new ListDemo { Id = 4, Name = "Object 4" }
            };

        public async Task<IActionResult> Index()
        {

            if(!User.Identity.IsAuthenticated)
            {
                var response = await _client.GetAsync("https://localhost:7003/avatar");
                if(response.IsSuccessStatusCode)
                {
                    var avatarBytes = await response.Content.ReadAsByteArrayAsync();
                    if (response.Content.Headers.TryGetValues("Content-Type", out var contentTypes))
                    {
                        var contentType = contentTypes.FirstOrDefault();

                        // Pass the Base64-encoded avatar string and content type to the view
                        ViewBag.Base64Avatar = Convert.ToBase64String(avatarBytes);
                        ViewBag.ContentType = contentType;
                    }
                }
               
            }
            ViewBag.list = new SelectList(list, "Id", "Name");
            return View();
        }
    }
}   
