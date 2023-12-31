﻿using System.Text.Json.Serialization;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Extensions;

namespace Web_153502_Logvinovich.Services
{
    public class SessionCart: Cart
    {  
        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
            SessionCart cart = session?.Get<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession? Session { get; set; }

        public override void AddToCart(Book book)
        {
            base.AddToCart(book);
            Session?.Set("Cart", this);
        }

        public override void ClearAll()
        {
            base.ClearAll();
            Session?.Set("Cart", this);
        }

        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            Session?.Set("Cart", this);
        }
    }
}
