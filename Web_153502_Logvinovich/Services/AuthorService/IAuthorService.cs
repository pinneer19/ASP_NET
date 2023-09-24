using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Domain.Models;

namespace Web_153502_Logvinovich.Services.AuthorService
{
	public interface IAuthorService
	{
		/// <summary>
		/// Получение списка всех авторов
		/// </summary>
		/// <returns></returns>
		public Task<ResponseData<List<Author>>> GetAuthorListAsync();
	}
}
