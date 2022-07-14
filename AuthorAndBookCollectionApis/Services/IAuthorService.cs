using AuthorAndBookCollectionApis.Entities;

namespace AuthorAndBookCollectionApis.Services
{
    public interface IAuthorService
    {
        public Task<Author> GetAuthorById(string authorId);
    }
}
