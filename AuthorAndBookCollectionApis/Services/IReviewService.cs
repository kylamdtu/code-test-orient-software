using AuthorAndBookCollectionApis.Entities;

namespace AuthorAndBookCollectionApis.Services
{
    public interface IReviewService
    {
        public Task GetReviewsByBook(Book book);
    }
}
