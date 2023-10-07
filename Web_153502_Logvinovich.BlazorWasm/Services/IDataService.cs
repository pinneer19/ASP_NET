using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Domain.Models;

namespace Web_153502_Logvinovich.BlazorWasm.Services
{
    public interface IDataService
    {
        // Событие о получении данных с API
        event Action DataLoaded;
        // Список авторов объектов
        List<Author> Authors { get; set; }
        //Список объектов
        List<Book> BookList { get; set; }
        // Признак успешного ответа на запрос к Api
        bool Success { get; set; }
        // Сообщение об ошибке
        string ErrorMessage { get; set; }
        // Количество страниц списка
        int TotalPages { get; set; }
        // Номер текущей страницы
        int CurrentPage { get; set; }
        /// <summary>
        /// Получение списка всех книг
        /// </summary>
        /// <param name="authorNormalizedName">нормализованное имя категории для фильтрации</param>
        /// <param name="pageNo">номер страницы списка</param>
        /// <returns></returns>
        public Task GetBookListAsync(string? authorNormalizedName, int pageNo = 1);
        /// <summary>
        /// Поиск книги по Id
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <returns>Найденный объект или null, если объект не найден</returns>
        public Task<ResponseData<Book>> GetBookByIdAsync(int id);
        /// <summary>
        /// Получение списка авторов
        /// </summary>
        /// <returns></returns>
        public Task GetAuthorListAsync();
    }
}
