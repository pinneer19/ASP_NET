using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data.SqlTypes;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Domain.Models;

namespace Web_153502_Logvinovich.Services.BookService
{
    public class ApiBookService : IBookService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly ILogger<ApiBookService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;
        public ApiBookService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiBookService> logger)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        public async Task<ResponseData<Book>> CreateBookAsync(Book book, IFormFile? formFile)
        {
            
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Books");
            var response = await _httpClient.PostAsJsonAsync(uri, book, _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response
                .Content
                .ReadFromJsonAsync<ResponseData<Book>>(_serializerOptions);

                if (formFile != null)
                {
                    await SaveImageAsync(data.Data.Id, formFile);
                }

                return data;
            }
            _logger.LogError($"-----> object not created. Error:{response.StatusCode}");
            return new ResponseData<Book>
            {
                Success = false,
                ErrorMessage = $"Объект не добавлен. Error:{response.StatusCode}"
            };
        }

        private async Task SaveImageAsync(int id, IFormFile image)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress.AbsoluteUri}Books/{id}")
            };
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(image.OpenReadStream());
            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;
            await _httpClient.SendAsync(request);
        }

        public async Task DeleteBookAsync(int id)
        {
            // DELETE: api/Books/5
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}books/");
            urlString.Append($"{id}");
            await _httpClient.DeleteAsync(new Uri(urlString.ToString()));
        }

        public async Task<ResponseData<Book>> GetBookByIdAsync(int id)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}books/");
            urlString.Append($"{id}");
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response
                        .Content
                        .ReadFromJsonAsync<ResponseData<Book>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<Book>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
            return new ResponseData<Book>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
            };
        }

        public async Task<ResponseData<ListModel<Book>>> GetBookListAsync(string? authorNormalizedName, int pageNo)
        {
            
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}books/");

            

            // добавить категорию в маршрут
            if (authorNormalizedName != null)
            {
                urlString.Append($"{authorNormalizedName}/");
            }

            if (authorNormalizedName == "all")
            {
                return await parseData(urlString);
            }

            // добавить номер страницы в маршрут
            if (pageNo > 1)
            {
                urlString.Append($"page{pageNo}");
            };
            // добавить размер страницы в строку запроса
            if (!_pageSize.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", _pageSize));
            }
            // отправить запрос к API
            return await parseData(urlString);
        }

        public async Task UpdateBookAsync(int id, Book book, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + $"Books/{id}");
            var response = await _httpClient.PutAsJsonAsync(uri, book, _serializerOptions);
            if (formFile != null)
            {
                await SaveImageAsync(id, formFile);
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> object not updated. Error:{response.StatusCode}");                
            }
            
        }

        private async Task<ResponseData<ListModel<Book>>> parseData(StringBuilder urlString)
        {
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response
                        .Content
                        .ReadFromJsonAsync<ResponseData<ListModel<Book>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<ListModel<Book>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
            return new ResponseData<ListModel<Book>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
            };
        }
    }
}
