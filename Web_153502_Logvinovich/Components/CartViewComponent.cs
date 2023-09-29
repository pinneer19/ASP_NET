using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Extensions;

namespace Web_153502_Logvinovich.Components
{
    [ViewComponent]
    public class CartViewComponent
    {
        //private readonly Cart _cart;
        private readonly HttpContext _context;
        public CartViewComponent(IHttpContextAccessor context)//Cart cart)
        {
            _context = context.HttpContext;
            //_cart = cart;
        }
        public HtmlString Invoke()
        {
            
            var cart = _context.Session.Get<Cart>("cart") ?? new();
            var total_price = cart.TotalPrice;
            var amount = cart.Count;
            return new HtmlString($"<a class=\"navbar-text ms-auto\" href=\"\\Cart\\Index\\\">{total_price} руб <i class=\"fa-solid fa-cart-shopping\"></i> ({amount})</a>");
        }
    }
}
