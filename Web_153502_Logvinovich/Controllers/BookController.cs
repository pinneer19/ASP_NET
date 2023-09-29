using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_153502_Logvinovich.Extensions;
using Web_153502_Logvinovich.Services.AuthorService;
using Web_153502_Logvinovich.Services.BookService;

namespace Web_153502_Logvinovich.Controllers
{
	public class BookController : Controller
	{

		private readonly IBookService _bookService;
		private readonly IAuthorService _authorService;

		public BookController(IBookService bookService, IAuthorService authorService) {
			_bookService = bookService;
			_authorService = authorService;
		}

		[Route("[controller]/{author?}")]
		public async Task<IActionResult> Index(string? author, int pageNo = 1)
		{
            var authors = _authorService.GetAuthorListAsync().Result.Data.AsEnumerable();
			var productResponse = await _bookService.GetBookListAsync(author, pageNo);
			if (!productResponse.Success)
				return NotFound(productResponse.ErrorMessage);
			
			ViewBag.authors = authors;
			ViewBag.books = productResponse.Data.Items.AsEnumerable();
			ViewBag.pageCount = productResponse.Data.TotalPages;
			ViewBag.currentAuthorName = author == null ? "Все" : authors.First(it => it.NormalizedName.Equals(author)).Name;

            ViewData["currentAuthor"] = author;
			ViewData["currentPage"] = productResponse.Data.CurrentPage;

			if(Request.IsAjaxRequest())
			{
				return PartialView("_BookPartial");
			}
            return View(productResponse.Data);
		}
	}
}
