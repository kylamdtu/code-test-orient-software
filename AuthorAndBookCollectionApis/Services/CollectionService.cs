using AuthorAndBookCollectionApis.Entities;
using System.Net;
using System.Text.Json;

namespace AuthorAndBookCollectionApis.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl;
        private readonly IAuthorService _authorService;
        private readonly IBookService _bookService;
        private readonly IReviewService _reviewService;
        public CollectionService(HttpClient client, IAuthorService authorService, IBookService bookService, IReviewService reviewService)
        {
            _client = client;
            baseUrl = "https://localhost:7037/api/authors/";
            _authorService = authorService;
            _bookService = bookService;
            _reviewService = reviewService;
        }

        public async Task<List<Author>> GetAuthorsByIds(List<string> authorIds)
        {
            try
            {
                var authors = new List<Author>();
                var getAuthorTasks = new List<Task<Author>>();
                var getBookTasks = new List<Task>();

                foreach (var authorId in authorIds)
                {
                    var endPoint = new Uri(string.Concat(baseUrl, authorId));
                    getAuthorTasks.Add(_authorService.GetAuthorById(authorId));
                }
                authors.AddRange(await Task.WhenAll(getAuthorTasks));

                foreach (var author in authors)
                {
                    var endPoint = new Uri(string.Concat(baseUrl, author.Id, "/books"));
                    getBookTasks.Add(_bookService.GetBooksByAuthor(author));
                }
                await Task.WhenAll(getBookTasks);
                return authors;
            }
            catch (Exception )
            {
                return new List<Author>();
            }
        }
    }

}
