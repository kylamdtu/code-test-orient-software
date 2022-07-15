namespace AuthorAndBookCollectionApis.Entities
{
    public class Book
    {
        public string Id { get; set; }
        public string AuthorId { get; set; }
        public string Name { get; set; }
        public double Stars { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
