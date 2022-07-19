using AuthorAndBookCollectionApis.Constants;
using AuthorAndBookCollectionApis.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace AuthorAndBookCollectionApis.Services
{
    public class CollectionService : ICollectionService
    {
        private readonly HttpClient _client;
        private readonly string baseUrl;
        private readonly ILibraryApiClients _libraryCLients;
        public CollectionService(HttpClient client, ILibraryApiClients authorService)
        {
            _client = client;
            baseUrl = "https://localhost:7037/api/authors/";
            _libraryCLients = authorService;
        }

        public async Task<List<Author>> GetAuthorsByIds(List<string> authorIds)
        {
            var authors = new List<Author>();
            var getAuthorTasks = new List<Task<Author>>();
            var getBookTasks = new List<Task>();

            foreach (var authorId in authorIds)
            {
                getAuthorTasks.Add(_libraryCLients.GetAuthorById(authorId));
            }
            authors.AddRange(await Task.WhenAll(getAuthorTasks));

            authors.RemoveAll(a => a is null);

            foreach (var author in authors)
            {
                getBookTasks.Add(Task.Run(async () => author.Books = await _libraryCLients.GetBooksByAuthor(author)));
            }

            await Task.WhenAll(getBookTasks);
            return authors;
        }
    }
}
