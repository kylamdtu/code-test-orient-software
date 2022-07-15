using AuthorAndBookCollectionApis.Entities;
using AuthorAndBookCollectionApis.Services;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Rhino.Mocks;
using System.Net;
using System.Text.Json;
using WireMock.Server;

namespace AuthorAndBookCollectionApis.UnitTest
{
    [TestFixture]
    public class CollectionServiceTest
    {
        private string baseUrl;
        private WireMockServer server;

        public CollectionServiceTest()
        {
            baseUrl = "https://localhost:7037/api/authors/";
        }

        [Test]
        public async Task it_should_get_the_right_authors_for_ids()
        {
            Mock<IAuthorService> authorServiceMock = new Mock<IAuthorService>();
            authorServiceMock.Setup(x => x.GetAuthorById(It.IsAny<string>())).Returns(Task.FromResult(new Author()
            {
                Id = "user1",
                Name = "User 1",
                Followers = 1231
            }));
            authorServiceMock.Setup(x => x.GetBooksByAuthor(It.IsAny<Author>())).Returns(Task.FromResult(new List<Book>()
            {
                new Book()
                                {
                                    Id = "book1",
                                    Name = "Book 1",
                                    Stars = 4.5,
                                    Reviews = new List<Review>()
                                    {
                                        new Review()
                                                            {
                                                                Reviewer = "Reviewer 1",
                                                                Content = "Great book!",
                                                            },
                                                            new Review()
                                                            {
                                                                Reviewer = "Reviewer 2",
                                                                Content = "I love this book",
                                                            }
                                    }
                                }
            }));

            var _service = new CollectionService(new HttpClient(), authorServiceMock.Object);

            var expectedResult = new List<Author>()
            {
                new Author()
                {
                     Id = "user1",
                    Name = "User 1",
                    Followers = 1231,
                    Books = new List<Book>
                    {
                        new Book()
                        {
                             Id = "book1",
                             Name = "Book 1",
                             Stars = 4.5,
                             Reviews = new List<Review>
                             {
                                 new Review()
                                    {
                                        Reviewer = "Reviewer 1",
                                        Content = "Great book!",
                                    },
                                    new Review()
                                    {
                                        Reviewer = "Reviewer 2",
                                        Content = "I love this book",
                                    }
                             }           
                        }
                    }
                }
            };

            var trueResult = await _service.GetAuthorsByIds(new List<string> { "user1" });

            Assert.AreEqual(expectedResult[0].Id, trueResult[0].Id);
            Assert.AreEqual(expectedResult[0].Books[0].Id, trueResult[0].Books[0].Id);
            Assert.AreEqual(expectedResult[0].Books[0].Reviews[0].Reviewer, trueResult[0].Books[0].Reviews[0].Reviewer);
        }
    }
}
