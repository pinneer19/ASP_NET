using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_153502_Logvinovich.Domain.Entities
{
    public class Cart
    {
        /// <summary>
        /// Список объектов в корзине
        /// key - идентификатор объекта
        /// </summary>
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        /// <summary>
        /// Добавить объект в корзину
        /// </summary>
        /// <param name="dish">Добавляемый объект</param>
        public virtual void AddToCart(Book book)
        {
            if(!CartItems.ContainsKey(book.Id))
            {
                CartItems.Add(book.Id, new CartItem { Book = book, Amount = 1 });
            }
            else
            {
                CartItems[book.Id].Amount++;
            }

        }
        /// <summary>
        /// Удалить объект из корзины
        /// </summary>
        /// <param name="id"> id удаляемого объекта</param>
        public virtual void RemoveItems(int id)
        {
            if(CartItems.ContainsKey(id))
            {
                if(CartItems[id].Amount > 1) { CartItems[id].Amount--; }
                else
                {
                    CartItems.Remove(id);
                }
            }
        }
        /// <summary>
        /// Очистить корзину
        /// </summary>
        public virtual void ClearAll()
        {
            CartItems.Clear();   
        }
        /// <summary>
        /// Количество объектов в корзине
        /// </summary>
        public int Count { get => CartItems.Sum(item => item.Value.Amount); }
        /// <summary>
        /// Общее количество калорий
        /// </summary>
        public double TotalPrice
        {
            get => CartItems.Sum(item => item.Value.Book.Price * item.Value.Amount);
        }
    }
}
