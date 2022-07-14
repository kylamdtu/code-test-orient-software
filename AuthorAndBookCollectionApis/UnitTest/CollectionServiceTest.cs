using AuthorAndBookCollectionApis.Entities;
using AuthorAndBookCollectionApis.Services;
using NUnit.Framework;
using Rhino.Mocks;
using System.Text.Json;
using WireMock.Server;

namespace AuthorAndBookCollectionApis.UnitTest
{
    [TestFixture]
    public class CollectionServiceTest
    {
        private string baseUrl;
        private ICollectionService _service;
        //private FakeHttpClient client;
        private WireMockServer server;

        public CollectionServiceTest()
        {
            baseUrl = "https://localhost:7037/api/authors/";
        }

        [SetUp]
        public void SetUp()
        {
            //client = MockRepository.GenerateStub<FakeHttpClient>();
            //client.Stub(_ => _.GetAsync(Arg<string>.Matches(x => x.EndsWith("user1"))))
            //    .Return(Task.FromResult(new HttpResponseMessage()
            //    {
            //        Content = JsonContent.Create(new Author()
            //        {
            //            Id = "user1",
            //            Name = "User 1",
            //            Followers = 1231
            //        })
            //    }));
            //client.Stub(_ => _.GetAsync(Arg<string>.Matches(x => x.EndsWith("user1/books"))))
            //    .Return(Task.FromResult(new HttpResponseMessage()
            //    {
            //        Content = JsonContent.Create(new Dictionary<string, List<Book>>()
            //        {
            //            {
            //                "user1",
            //                new List<Book>()
            //                {
            //                    new Book()
            //                    {
            //                        Id = "book1",
            //                        Name = "Book 1",
            //                        Stars = 4.5,
            //                    }

            //                }
            //            },
            //            {
            //                "user2",
            //                new List<Book>()
            //                {
            //                    new Book()
            //                    {
            //                        Id = "book2",
            //                        Name = "Book 2",
            //                        Stars = 4.5,
            //                    }
            //                }
            //            }
            //        })
            //    }));
            //client.Stub(_ => _.GetAsync(Arg<string>.Matches(x => x.EndsWith("user1/books/book1/reviews"))))
            //    .Return(Task.FromResult(new HttpResponseMessage()
            //    {
            //        Content = JsonContent.Create(new Dictionary<string, List<Review>>()
            //        {
            //            {
            //                "book1",
            //                 new List<Review>()
            //                    {
            //                        new Review()
            //                        {
            //                            Reviewer = "Reviewer 1",
            //                            Content = "Great book!",
            //                        },
            //                        new Review()
            //                        {
            //                            Reviewer = "Reviewer 2",
            //                            Content = "I love this book",
            //                        }
            //                    }
            //            },
            //            {
            //                "book2",
            //                new List<Review>()
            //                    {
            //                        new Review()
            //                        {
            //                            Reviewer = "Reviewer 3",
            //                            Content = "Great book!",
            //                        },
            //                        new Review()
            //                        {
            //                            Reviewer = "Reviewer 4",
            //                            Content = "I love this book",
            //                        }
            //                    }
            //            }
            //        })
            //    }));
            //_service = new CollectionService(client);
        }

        [Test]
        public async Task it_should_get_the_right_authors_for_ids()
        {
            var expectedResult = JsonSerializer.Serialize(new Author()
            { 
                Id = "user1",
                Name = "User 1",
                Followers = 1231,
                Books = new List<Book>()
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
                }
            });

            var trueResult = await _service.GetAuthorsByIds(new List<string> { "user1" });

            Assert.Equals(expectedResult, trueResult);
        }
    }
}
