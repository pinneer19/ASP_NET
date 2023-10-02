using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Domain.Models;
using Web_153502_Logvinovich.Api.Services.BookService;
using Web_153502_Logvinovich.Api.Data;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;
using Azure.Core;
using System.Drawing;
using System.Net.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Web_153502_Logvinovich.API.Services
{
    public class BookService : IBookServiceApi
    {
        private readonly int _maxPageSize = 20;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<BookService> _logger;
        public BookService(ApplicationDbContext context, IWebHostEnvironment env, IHttpContextAccessor accessor, ILogger<BookService> logger)
        {            
            _context = context;
            _environment = env;
            _httpContextAccessor = accessor;
            _logger = logger;
        }

        public async Task<ResponseData<Book>> CreateBookAsync(Book book, IFormFile? formFile)
        {
            try
            {                
                _context.Entry(book.Author).State = EntityState.Unchanged;
                _context.Books.Add(book);
                
                await _context.SaveChangesAsync();
                return new ResponseData<Book> { Data = book, Success = true };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null) _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<ResponseData<Book>> GetBookByIdAsync(int id)
        {
            return new ResponseData<Book> { Data = await _context.Books.Include(x => x.Author).FirstAsync(x=> x.Id == id) };
        }

    public async Task<ResponseData<ListModel<Book>>> GetBookListAsync(string? authorNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize) pageSize = _maxPageSize;
            var query = _context.Books.Include(book => book.Author).AsQueryable();
            var dataList = new ListModel<Book>();
            if (authorNormalizedName != "all")
            {
                query = query.Where(d => authorNormalizedName == null || d.Author.NormalizedName.Equals(authorNormalizedName));
            }
            // количество элементов в списке
            var count = query.Count();
            if (count == 0)
            {
                return new ResponseData<ListModel<Book>>
                {
                    Data = dataList                     
                };
            }
            // количество страниц
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
            {
                return new ResponseData<ListModel<Book>>
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "No such page"
                };
            }
            dataList.Items = await query
                                    .Skip((pageNo - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;

            var response = new ResponseData<ListModel<Book>>
            {
                Data = dataList
            };
            return response;
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var responseData = new ResponseData<string>();
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                responseData.Success = false;
                responseData.ErrorMessage = "No item found";
                return responseData;
            }
            var host = "https://" + _httpContextAccessor.HttpContext.Request.Host;
            var imageFolder = Path.Combine(_environment.WebRootPath, "Images");
            if (formFile != null)
            {
                // Удалить предыдущее изображение
                if (!string.IsNullOrEmpty(book.Image))
                {
                    var prevImage = Path.GetFileName(book.Image);
                    File.Delete(Path.Combine(imageFolder, prevImage));
                }
                // Создать имя файла
                var ext = Path.GetExtension(formFile.FileName);
                var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
                // Сохранить файл
                using (var fileStream = new FileStream(Path.Combine(imageFolder, fName), FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }
                
                // Указать имя файла в объекте
                book.Image = $"{host}/Images/{fName}";
                await _context.SaveChangesAsync();
            }
            responseData.Data = book.Image;
            return responseData;
        }

        public async Task UpdateBookAsync(int id, Book book, IFormFile? formFile)
        {
            try
            {
                //_context.Entry(book.Author).State = EntityState.Modified;
                _context.Books.Update(book);

                await _context.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
 