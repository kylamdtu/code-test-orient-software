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
    }
}
