using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_153502_Logvinovich.Data;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Services.AuthorService;
using Web_153502_Logvinovich.Services.BookService;

namespace Web_153502_Logvinovich.Areas.Admin.Views.Books
{
    public class CreateModel : PageModel
    {
        private readonly IBookService _service;
        private readonly IAuthorService _author_service;
        public CreateModel(IBookService service, IAuthorService authorService)
        {
            _service = service;
            _author_service = authorService;   
        }

        public async Task OnGetAsync()
        {
            await InitializeAuthors();
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }


        [BindProperty]
        public int AuthorId { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        { 
            // почему то не валидирует даже после добавления автора
            if (Book == null || Image == null)
            {
                await InitializeAuthors();
                return Page();
            }
            Book.Author = _author_service.GetAuthorListAsync().Result.Data.FirstOrDefault(x => x.Id == AuthorId) ?? throw new Exception("Author was not found");
            await _service.CreateBookAsync(Book, Image);
            
            return RedirectToPage("./Index");
        }

        private async Task InitializeAuthors()
        {
            var authors = await _author_service.GetAuthorListAsync();
            ViewData["Authors"] = authors.Data.Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString() });
        }
    }
}
