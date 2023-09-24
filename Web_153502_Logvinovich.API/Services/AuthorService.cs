using Microsoft.EntityFrameworkCore;
using Web_153502_Logvinovich.Api.Data;
using Web_153502_Logvinovich.Api.Services.AuthorService;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Domain.Models;


namespace Web_153502_Logvinovich.API.Services
{
    public class AuthorService : IAuthorServiceApi
    {
        private readonly ApplicationDbContext _context;

        public AuthorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<ResponseData<List<Author>>> GetAuthorListAsync()
        {
            return Task.FromResult(new ResponseData<List<Author>> { Data = _context.Authors.ToList() });
        }
        
    }
}
