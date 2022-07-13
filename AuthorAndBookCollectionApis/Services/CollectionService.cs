using AuthorAndBookCollectionApis.Entities;
using System.Net;
using System.Text.Json;

namespace AuthorAndBookCollectionApis.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl;
        public CollectionService(HttpClient client)
        {
            _client = client;
            baseUrl = "https://localhost:7037/api/authors/";
        }

        public async Task<List<Author>> GetAuthorsByIds(List<string> authorIds)
        {
            try
            {
                var authors = new List<Author>();
                var tasks = new List<Task>();

                foreach (var authorId in authorIds)
                {
                    var endPoint = new Uri(string.Concat(baseUrl, authorId));
                    tasks.Add(GetAuthorById(authors, endPoint));
                }
                await Task.WhenAll(tasks);
                //tasks.Clear();

                foreach (var author in authors)
                {
                    var endPoint = new Uri(string.Concat(baseUrl, author.Id, "/books"));
                    tasks.Add(GetBooksByAuthor(author, endPoint));
                }
                await Task.WhenAll(tasks);
                return authors;
            }
            catch (Exception )
            {
                return new List<Author>();
            }
        }

        private async Task GetBooksByAuthor(Author author, Uri uri)
        {
            try
            {
                var response = await _client.GetAsync(uri);
                author.Books = await response.Content.ReadFromJsonAsync<List<Book>>() ?? new List<Book>();
                var tasks = new List<Task>();
                foreach (var book in author.Books)
                {
                    var endPoint = string.Concat(baseUrl, author.Id, "/books/", book.Id, "/reviews");
                    tasks.Add(GetReviewByBookId(book, endPoint));
                }
                await Task.WhenAll(tasks);
            }
            catch (Exception)
            {
                author.Books = new List<Book>();
            }
        }

        private async Task GetReviewByBookId(Book book, string uri)
        {
            try
            {
                var response = await _client.GetAsync(uri);
                book.Reviews = await response.Content.ReadFromJsonAsync<List<Review>>();
            }
            catch (Exception)
            {
                book.Reviews = new List<Review>();
            }
        }

        private async Task GetAuthorById(List<Author> authors, Uri uri)
        {
            try
            {
                var response = await _client.GetAsync(uri);
                authors.Add(response.Content.ReadFromJsonAsync<Author>().Result ?? new Author());
                Thread.Sleep(20000);
            }
            catch (Exception)
            {
                authors.Add(new Author());
            }
        }
    }

}
