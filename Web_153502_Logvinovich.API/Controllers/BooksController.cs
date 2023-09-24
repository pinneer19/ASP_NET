using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_153502_Logvinovich.Api.Data;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Domain.Models;
using Web_153502_Logvinovich.Api.Services.BookService;
using Microsoft.AspNetCore.Authorization;

namespace Web_153502_Logvinovich.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookServiceApi _bookService;
        private readonly string _imagesPath;
        private readonly string _appUri;

        public BooksController(IBookServiceApi bookService, IWebHostEnvironment env, IConfiguration configuration)
        {
            _bookService = bookService;
            _imagesPath = Path.Combine(env.WebRootPath, "Images");
            _appUri = configuration.GetSection("appUri").Value!;
        }

        // GET: api/Books
        // [Route("api/Books/{author}/{pageNo}")]
        [HttpGet]
        [HttpGet("{author}/page{pageNo:int}")]
        [HttpGet("page{pageNo:int}")]
        [HttpGet("{author}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseData<IEnumerable<Book>>>> GetBooks(string? author, int pageNo = 1, int pageSize = 3)
        {
            return Ok(await _bookService.GetBookListAsync(author, pageNo, pageSize));
        }

        //GET: api/Books/5
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            //_.Entry(book).State = EntityState.Modified;

            try
            {
                await _bookService.UpdateBookAsync(id, book, null);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ResponseData<Book>> PostBook(Book book)
        {
            var newBook = await _bookService.CreateBookAsync(book, null);
            return newBook;
        }

        [HttpPost("id")]
        [Authorize]
        public async Task<ResponseData<string>> PostImageBook(int id, [FromForm] IFormFile formFile)
        {            
            return await _bookService.SaveImageAsync(id , formFile);
        }

        //// DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return Ok();
        }

        private bool BookExists(int id)
        {
            return _bookService.GetBookByIdAsync(id) != null;
        }

        // POST: api/Dishes/5
        [HttpPost("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseData<string>>> PostImage(int id, IFormFile formFile)
        {
            var response = await _bookService.SaveImageAsync(id, formFile);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
