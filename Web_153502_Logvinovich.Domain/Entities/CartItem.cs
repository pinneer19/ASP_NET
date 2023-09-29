using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_153502_Logvinovich.Domain.Entities
{
    public class CartItem
    {
        public Book? Book { get; set; }
        public int Amount { get; set; }
    }
}
