using Microsoft.AspNetCore.Mvc;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Api.Services.AuthorService;

namespace Web_153502_Logvinovich.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorServiceApi _authorService;
        
        public AuthorsController(IAuthorServiceApi authorService)
        {
            _authorService = authorService;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return Ok(await _authorService.GetAuthorListAsync());
        }
    }
}
