﻿using Microsoft.AspNetCore.Mvc;
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

            ViewBag.list = new SelectList(list, "Id", "Name");
            return View();
        }
    }
}   
