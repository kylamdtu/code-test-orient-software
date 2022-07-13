using AuthorAndBookCollectionApis.Entities;

namespace AuthorAndBookCollectionApis.Services
{
    public interface ICollectionService
    {
        public Task<List<Author>> GetAuthorsByIds(List<string> authorIds);
    }
}
