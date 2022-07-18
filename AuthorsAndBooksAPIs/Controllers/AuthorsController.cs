using AuthorsAndBooksAPIs.Entities;
using AuthorsAndBooksAPIs.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthorsAndBooksAPIs.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        AuthorService service;
        public AuthorsController()
        {
            service = new AuthorService();
        }
        [HttpGet]
        [Route("{authorId}")]
        public async Task<ActionResult<Author>> GetAuthorById([FromRoute]string authorId) 
            => await service.GetAuthorByAuthorId(authorId);

        [HttpGet]
        [Route("{authorId}/books")]
        public async Task<ActionResult<List<Book>>> GetBooksByAuthorId([FromRoute]string authorId)
            => await Task.Run(() => service.GetBooksByAuthorId(authorId));

        [HttpGet]
        [Route("{authorId}/books/{bookId}/reviews")]
        public async Task<ActionResult<List<Review>>> GetReviewsByBookId([FromRoute] string bookId)
            => await Task.Run(() => service.GetReviewsByBookId(bookId));
    }
}
