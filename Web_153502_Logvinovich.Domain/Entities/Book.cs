using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_153502_Logvinovich.Domain.Entities
{
	public class Book
	{
		public int Id { get; set; }
		[Display(Name = "Название")]
		public string Name { get; set; }
        [Display(Name = "Автор")]
        public Author Author { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
		[Display(Name = "Картинка")]
		public string? Image { get; set; }
        [Display(Name = "Цена")]
        public int Price { get; set; }	

	}
}
