using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web_153502_Logvinovich.Data;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Services.BookService;

namespace Web_153502_Logvinovich.Areas.Admin.Views.Books
{
    public class DetailsModel : PageModel
    {
        private readonly IBookService _service;

        public DetailsModel(IBookService service)
        {
            _service = service;
        }

        public Book Book { get; set; } = default!;
        
     

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = (await _service.GetBookByIdAsync(id.Value)).Data;
            if (book == null)
            {
                return NotFound();
            }
            else
            {
                Book = book;
            }
            return Page();
        }
    }
}
