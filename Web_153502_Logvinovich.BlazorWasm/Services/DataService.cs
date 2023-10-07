
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Domain.Models;

namespace Web_153502_Logvinovich.BlazorWasm.Services
{
    public class DataService : IDataService
    {
        public List<Author> Authors { get; set; }
        public List<Book> BookList { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        private readonly int _pageSize;
        private readonly HttpClient _httpClient;
        private readonly IAccessTokenProvider _tokenProvider;

        public event Action DataLoaded;

        public DataService(IConfiguration configuration, HttpClient httpClient, IAccessTokenProvider tokenProvider)
        {
            _pageSize = configuration.GetValue<int>("PageSize");
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
        }

        public async Task GetAuthorListAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseData<List<Author>>>("Authors");
            
            Success = response.Success;
            ErrorMessage = response.ErrorMessage;
            
            if(Success)
            {
                Authors = response.Data;
            }
            DataLoaded.Invoke();
        }

        public async Task<ResponseData<Book>> GetBookByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseData<Book>>($"Books/{id}");

            Success = response.Success;
            ErrorMessage = response.ErrorMessage;
            DataLoaded.Invoke();
            return response;
            
        }

        public async Task GetBookListAsync(string? authorNormalizedName, int pageNo = 1)
        {
            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);
                if (authorNormalizedName == null) authorNormalizedName = "all";
                var response = await _httpClient.GetFromJsonAsync<ResponseData<ListModel<Book>>>($"Books/{authorNormalizedName}/page{pageNo}");
                Success = response.Success;
                ErrorMessage = response.ErrorMessage;
                if (Success)
                {
                    BookList = response.Data.Items;
                    TotalPages = response.Data.TotalPages;
                    CurrentPage = response.Data.CurrentPage;
                }
                DataLoaded.Invoke();
            }
        }
    }
}
