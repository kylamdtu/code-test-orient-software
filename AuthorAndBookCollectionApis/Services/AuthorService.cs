using AuthorAndBookCollectionApis.Constants;
using AuthorAndBookCollectionApis.Entities;

namespace AuthorAndBookCollectionApis.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly HttpClient _client;
        public AuthorService(HttpClient client)
        {
            _client = client;
        }

        public async Task<Author> GetAuthorById(string authorId)
        {
            try
            {
                var response = await _client.GetAsync(string.Concat(UriConstants.ROOT_URL, authorId));
                Thread.Sleep(2000);
                return response.Content.ReadFromJsonAsync<Author>().Result ?? new Author();
            }
            catch (Exception)
            {
                return new Author();
            }
        }

        public async Task<List<Book>> GetBooksByAuthor(Author author)
        {
            try
            {
                var response = await _client.GetAsync(string.Concat(UriConstants.ROOT_URL, author.Id, "/books"));
                var books = response.Content.ReadFromJsonAsync<List<Book>>().Result ?? new List<Book>();
                var tasks = new List<Task>();
                foreach (var book in books)
                {
                    book.AuthorId = author.Id;
                    tasks.Add(Task.Run(async () => book.Reviews = await GetReviewsByBook(book)));
                }
                await Task.WhenAll(tasks);
                return books;
            }
            catch (Exception)
            {
                return new List<Book>();
            }
        }

        public async Task<List<Review>> GetReviewsByBook(Book book)
        {
            try
            {
                var response = await _client.GetAsync(string.Concat(UriConstants.ROOT_URL, book.AuthorId, "/books/", book.Id, "/reviews"));
                return response.Content.ReadFromJsonAsync<List<Review>>().Result ?? new List<Review>();
            }
            catch (Exception)
            {
                return new List<Review>();
            }
        }
    }
}
