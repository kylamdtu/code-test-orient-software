using AuthorAndBookCollectionApis.Constants;
using AuthorAndBookCollectionApis.Entities;

namespace AuthorAndBookCollectionApis.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;
        private readonly IReviewService _bookService;

        public BookService(HttpClient httpClient, IReviewService reviewService)
        {
            _httpClient = httpClient;
            _bookService = reviewService;
        }

        public async Task GetBooksByAuthor(Author author)
        {
            try
            {
                var response = await _httpClient.GetAsync(string.Concat(UriConstants.ROOT_URL, author.Id, "/books"));
                var books = response.Content.ReadFromJsonAsync<List<Book>>().Result ?? new List<Book>();
                var tasks = new List<Task>();
                foreach (var book in books)
                {   
                    book.AuthorId = author.Id;
                    tasks.Add(_bookService.GetReviewsByBook(book));
                }
                await Task.WhenAll(tasks);
                author.Books = books;
            }
            catch (Exception)
            {
                author.Books = new List<Book>();
            }
        }
    }
}
