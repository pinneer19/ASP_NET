using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Web_153502_Logvinovich.Domain.Entities;
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
		//[Route("[controller]")]
		[Route("Catalog/{author=all}/{pageNo=1}")]
		public async Task<IActionResult> Index(string? author, int pageNo = 1)
		{
            var authorsResponse = await _authorService.GetAuthorListAsync();

            // если список не получен, вернуть код 404
            if (!authorsResponse.Success)
                return NotFound(authorsResponse.ErrorMessage);

            ViewBag.authors = authorsResponse.Data.AsEnumerable();

			ViewData["currentAuthor"] = author.IsNullOrEmpty() || author == "all" ? "Все" : authorsResponse.Data.FirstOrDefault(it => it.NormalizedName.Equals(author))?.Name;
			ViewBag.author = author;
            var productResponse = await _bookService.GetBookListAsync(author, pageNo);

			if (!productResponse.Success)
				return NotFound(productResponse.ErrorMessage);
			
			if(Request.IsAjaxRequest())
			{
				return PartialView("_BookPartial", productResponse.Data);
			}
            return View(productResponse.Data);
		}
	}
}
