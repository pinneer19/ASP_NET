using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Domain.Models;

namespace Web_153502_Logvinovich.Api.Services.AuthorService
{
	public interface IAuthorServiceApi
	{
		/// <summary>
		/// Получение списка всех авторов
		/// </summary>
		/// <returns></returns>
		public Task<ResponseData<List<Author>>> GetAuthorListAsync();
	}
}
