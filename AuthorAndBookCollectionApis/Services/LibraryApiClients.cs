using AuthorAndBookCollectionApis.Constants;
using AuthorAndBookCollectionApis.Entities;
using System.Net;
using System.Web.Http;

namespace AuthorAndBookCollectionApis.Services
{
    public class LibraryApiClients : ILibraryApiClients
    {
        private readonly HttpClient _client;
        private readonly ILogger _logger;
        public LibraryApiClients(HttpClient client, ILogger<ILibraryApiClients> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<Author> GetAuthorById(string authorId)
        {
            var uri = string.Concat(UriConstants.ROOT_URL, authorId);
            var response = await _client.GetAsync(string.Concat(uri));
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var errorMsg =
                    $"Error getting from {uri}; Status Code: {response.StatusCode}; Error message: {response.ReasonPhrase}; Content: {content}";
                _logger.LogInformation(errorMsg);
                return null;
            }
            return response.Content.ReadFromJsonAsync<Author>().Result;
        }

        public async Task<List<Book>> GetBooksByAuthor(Author author)
        {
            var uri = string.Concat(UriConstants.ROOT_URL, author.Id, "/books");
            var response = await _client.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync();
                var errorMsg =
                    $"Error getting from {uri}; Status Code: {response.StatusCode}; Error message: {response.ReasonPhrase}; Content: {content}";
                _logger.LogInformation(errorMsg);
                return new List<Book>();
            }

            var tasks = new List<Task>();
            var books = await response.Content.ReadFromJsonAsync<List<Book>>();
            if (books is not null)
            {
                foreach (var book in books)
                {
                    if (book != null)
                    {
                        book.AuthorId = author.Id;
                        tasks.Add(Task.Run(async () => book.Reviews = await GetReviewsByBook(book)));
                    }
                }
            }
            await Task.WhenAll(tasks);
            return books;
        }

        public async Task<List<Review>> GetReviewsByBook(Book book)
        {
            var uri = string.Concat(UriConstants.ROOT_URL, book.AuthorId, "/books/", book.Id, "/reviews");
            var response = await _client.GetAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var errorMsg =
                    $"Get error from: {uri}; Status code: {response.StatusCode}; Error Message: {response.ReasonPhrase}; Content: {content}";
                _logger.LogInformation(errorMsg);
                return new List<Review>();
            }
            return response.Content.ReadFromJsonAsync<List<Review>>().Result ?? new List<Review>();
        }
    }
}
