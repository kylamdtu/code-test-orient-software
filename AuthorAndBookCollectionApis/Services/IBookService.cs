using AuthorAndBookCollectionApis.Entities;

namespace AuthorAndBookCollectionApis.Services
{
    public interface IBookService
    {
        public Task GetBooksByAuthor(Author author);
    }
}
