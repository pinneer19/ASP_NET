using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Extensions;
using Web_153502_Logvinovich.Services.BookService;

namespace Web_153502_Logvinovich.Controllers
{
    public class CartController : Controller
    {
        //private readonly Cart _cart;
        private readonly IBookService _bookService;
        private readonly HttpContext _context;

        public CartController(IBookService bookService)
        {
            //_cart = cart;
            _bookService = bookService;
        }

        public IActionResult Index()
        {
            Cart cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            return View(cart.CartItems);
        }

        [Authorize]
        [Route("[controller]/add/{id:int}")]
        public async Task<IActionResult> AddBookToCart(int id, string returnurl)
        {
            Cart cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            var book = await _bookService.GetBookByIdAsync(id) ?? throw new Exception("No book with such id"); 
            cart.AddToCart(book.Data);
            HttpContext.Session.Set<Cart>("cart", cart);
            return Redirect(returnurl);
        }

        [Authorize]
        [Route("[controller]/delete/{id:int}")]
        public async Task<IActionResult> DeleteBookFromCart(int id)
        {
            Cart cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            cart.RemoveItems(id);
            HttpContext.Session.Set<Cart>("cart", cart);
            return View("Index", cart.CartItems);
        }


        [Authorize]
        [Route("[controller]/clear")]
        public async Task<IActionResult> ClearCart()
        {
            Cart cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            cart.ClearAll();
            HttpContext.Session.Set<Cart>("cart", cart);
            return View("Index", cart.CartItems);
        }

    }
}
