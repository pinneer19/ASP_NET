using System.Net.Http;
using System.Text.Json;
using System.Text;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Domain.Models;

namespace Web_153502_Logvinovich.Services.AuthorService
{
    public class ApiAuthorService : IAuthorService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _serializerOptions;
        public ApiAuthorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<ResponseData<List<Author>>> GetAuthorListAsync()
        {
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Authors/");

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response
                        .Content
                        .ReadFromJsonAsync<ResponseData<List<Author>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    return new ResponseData<List<Author>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }

            return new ResponseData<List<Author>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
            };
        }
    }
}
