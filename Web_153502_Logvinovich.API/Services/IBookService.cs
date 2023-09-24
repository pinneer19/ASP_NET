using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Domain.Models;

namespace Web_153502_Logvinovich.Api.Services.BookService
{
	public interface IBookServiceApi
	{
        /// <summary>
        /// Получение списка всех объектов
        /// </summary>
        /// <param name="authorNormalizedName">нормализованное имя категории для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param> 
        /// /// <param name="pageSize">количество объектов на странице</param> 
        /// <returns></returns>
        public Task<ResponseData<ListModel<Book>>> GetBookListAsync(string? authorNormalizedName, int pageNo, int pageSize);
		
		/// <summary>
		/// Поиск объекта по Id
		/// </summary>
		/// <param name="id">Идентификатор объекта</param>
		/// <returns>Найденный объект или null, если объект не найден</returns>
		public Task<ResponseData<Book>> GetBookByIdAsync(int id);

		/// <summary>
		/// Обновление объекта
		/// </summary>
		/// <param name="id">Id изменяемомго объекта</param>
		/// <param name="book">объект с новыми параметрами</param>
		/// <param name="formFile">Файл изображения</param>
		/// <returns></returns>
		public Task UpdateBookAsync(int id, Book book, IFormFile? formFile);
		
		/// <summary>
		/// Удаление объекта
		/// </summary>
		/// <param name="id">Id удаляемомго объекта</param>
		/// <returns></returns>
		public Task DeleteBookAsync(int id);
		
		/// <summary>
		/// Создание объекта
		/// </summary>
		/// <param name="book">Новый объект</param>
		/// <param name="formFile">Файл изображения</param>
		/// <returns>Созданный объект</returns>
		public Task<ResponseData<Book>> CreateBookAsync(Book book, IFormFile? formFile);

        /// <summary>
        /// Сохранить файл изображения для объекта
        /// </summary>
        /// <param name="id">Id объекта</param>
        /// <param name="formFile">файл изображения</param>
        /// <returns>Url к файлу изображения</returns
        public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);

    }
}
