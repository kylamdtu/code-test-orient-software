using AuthorAndBookCollectionApis.Entities;

namespace AuthorAndBookCollectionApis.Services
{
    public interface IAuthorService
    {
        public Task<Author> GetAuthorById(string authorId);
        public Task<List<Book>> GetBooksByAuthor(Author author);
        public Task<List<Review>> GetReviewsByBook(Book book);
    }
}
