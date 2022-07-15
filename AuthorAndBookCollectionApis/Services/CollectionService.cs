using AuthorAndBookCollectionApis.Constants;
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
        public CollectionService(HttpClient client, IAuthorService authorService)
        {
            _client = client;
            baseUrl = "https://localhost:7037/api/authors/";
            _authorService = authorService;
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
                    getAuthorTasks.Add(_authorService.GetAuthorById(authorId));
                }
                authors.AddRange(await Task.WhenAll(getAuthorTasks));

                foreach (var author in authors)
                {
                    getBookTasks.Add(Task.Run(async () => author.Books = await _authorService.GetBooksByAuthor(author)));
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
