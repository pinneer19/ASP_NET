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
        private readonly Cart _cart;
        private readonly IBookService _bookService;        

        public CartController(IBookService bookService, Cart cart)
        {
            _bookService = bookService;
            _cart = cart;
        }

        public IActionResult Index()
        {
            return View(_cart.CartItems);
        }

        [Authorize]
        [Route("[controller]/add/{id:int}")]
        public async Task<IActionResult> AddBookToCart(int id, string returnurl)
        {
            var book = await _bookService.GetBookByIdAsync(id) ?? throw new Exception("No book with such id"); 
            _cart.AddToCart(book.Data);
            return Redirect(returnurl);
        }

        [Authorize]
        [Route("[controller]/delete/{id:int}")]
        public async Task<IActionResult> DeleteBookFromCart(int id)
        {            
            _cart.RemoveItems(id);       
            return View("Index", _cart.CartItems);
        }


        [Authorize]
        [Route("[controller]/clear")]
        public async Task<IActionResult> ClearCart()
        {
            _cart.ClearAll();   
            return View("Index", _cart.CartItems);
        }

    }
}
