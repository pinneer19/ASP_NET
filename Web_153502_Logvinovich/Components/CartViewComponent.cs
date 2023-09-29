using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Extensions;

namespace Web_153502_Logvinovich.Components
{
    [ViewComponent]
    public class CartViewComponent
    {
        private readonly Cart _cart;
        
        public CartViewComponent(Cart cart)
        {
            _cart = cart;
        }
        public HtmlString Invoke()
        {
            var total_price = _cart.TotalPrice;
            var amount = _cart.Count;
            return new HtmlString($"<a class=\"navbar-text ms-auto\" href=\"\\Cart\\Index\\\">{total_price} руб <i class=\"fa-solid fa-cart-shopping\"></i> ({amount})</a>");
        }
    }
}
