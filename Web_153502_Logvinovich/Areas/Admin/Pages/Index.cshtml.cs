using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Domain.Models;
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

        public ListModel<Book> Book { get;set; } = default!;

        public async Task OnGetAsync(int pageNo = 1)
        {
            
            var books = await _service.GetBookListAsync(null, pageNo);
            Book = books.Data;  
        }
    }
}
