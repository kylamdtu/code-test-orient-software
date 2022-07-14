using AuthorAndBookCollectionApis.Constants;
using AuthorAndBookCollectionApis.Entities;

namespace AuthorAndBookCollectionApis.Services
{
    public class ReviewService : IReviewService
    {
        HttpClient _client;

        public ReviewService(HttpClient client)
        {
            _client = client;
        }

        public async Task GetReviewsByBook(Book book)
        {
            try
            {
                var response = await _client.GetAsync(string.Concat(UriConstants.ROOT_URL, book.AuthorId, "/books/", book.Id, "/reviews"));
                book.Reviews = response.Content.ReadFromJsonAsync<List<Review>>().Result ?? new List<Review>();
            }
            catch (Exception)
            {
                book.Reviews = new List<Review>();
            }
        }
    }
}
