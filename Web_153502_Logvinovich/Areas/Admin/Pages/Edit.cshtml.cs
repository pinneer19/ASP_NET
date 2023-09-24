using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Web_153502_Logvinovich.Data;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Services.AuthorService;
using Web_153502_Logvinovich.Services.BookService;

namespace Web_153502_Logvinovich.Areas.Admin.Views.Books
{
    public class EditModel : PageModel
    {
        private readonly IBookService _service;
        private readonly IAuthorService _author_service;
        public EditModel(IBookService service, IAuthorService authorService)
        {
            _service = service;
            _author_service = authorService;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        [BindProperty]
        public int AuthorId { get; set; }

        [BindProperty]
        //[DisplayName("Картинка")]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await getBook(id);
            if (book == null)
            {
                return NotFound();
            }
            Book = book;
            await InitializeAuthors();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (Book == null || Image == null)
            {
                await InitializeAuthors();
                return Page();
            }

            try
            {
                if (Image == null) Book.Image = (await getBook(Book.Id)).Image;
                Book.Author = _author_service.GetAuthorListAsync().Result.Data.FirstOrDefault(x => x.Id == AuthorId) ?? throw new Exception("Author was not found");
                await _service.UpdateBookAsync(Book.Id, Book, Image);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(Book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BookExists(int id)
        {
            return _service.GetBookListAsync(null, 1).Result.Data.Items.Any(x => x.Id == id);
        }

        private async Task InitializeAuthors()
        {
            var authors = await _author_service.GetAuthorListAsync();
            ViewData["Authors"] = authors.Data.Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString() });
        }

        private async Task<Book> getBook(int? id)
        {
            return (await _service.GetBookByIdAsync(id.Value)).Data;
        }
    }

}
