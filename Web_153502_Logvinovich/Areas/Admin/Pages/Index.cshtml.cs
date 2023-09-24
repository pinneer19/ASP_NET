using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Services.BookService;

namespace Web_153502_Logvinovich.Areas.Admin.Views.Books
{
    public class IndexModel : PageModel
    {
        private readonly IBookService _service;


        public IndexModel(IBookService service)
        {
            _service = service;
        }

        public IList<Book> Book { get;set; } = default!;

        public async Task OnGetAsync()
        {
            
            var books = await _service.GetBookListAsync("all", 1);
            Book = books.Data.Items;  
        }
    }
}
