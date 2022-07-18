using AuthorsAndBooksAPIs.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web.Http;

namespace AuthorsAndBooksAPIs.Services
{
    public class AuthorService
    {
        List<Author> authors;
        Dictionary<string, List<Book>> books;
        Dictionary<string, List<Review>> reviews;
        public AuthorService()
        {
            authors = new List<Author>()
            {
                new Author()
                {
                    Id = "user1",
                    Name = "User 1",
                    Followers = 1231
                },
                new Author()
                {
                    Id = "user2",
                    Name = "User 2",
                    Followers = 1232
                },
                new Author()
                {
                    Id = "user3",
                    Name = "User 3",
                    Followers = 1233
                },
                new Author()
                {
                    Id = "user4",
                    Name = "User 4",
                    Followers = 1234
                },
                new Author()
                {
                    Id = "user5",
                    Name = "User 5",
                    Followers = 1235
                },
            };

            books = new Dictionary<string, List<Book>>()
            {
                {
                    "user1",
                    new List<Book>()
                    {
                        new Book()
                        {
                            Id = "book1",
                            Name = "Book 1",
                            Stars = 4.5,
                        }

                    }
                },
                {
                    "user2",
                    new List<Book>()
                    {
                        new Book()
                        {
                            Id = "book2",
                            Name = "Book 2",
                            Stars = 4.5,
                        },
                        new Book()
                        {
                            Id = "book3",
                            Name = "Book 3",
                            Stars = 4.9,
                        }
                    }
                }
            };

            reviews = new Dictionary<string, List<Review>>()
            {
                {
                    "book1",
                     new List<Review>()
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
                },
                {
                    "book2",
                    new List<Review>()
                        {
                            new Review()
                            {
                                Reviewer = "Reviewer 3",
                                Content = "Great book!",
                            },
                            new Review()
                            {
                                Reviewer = "Reviewer 4",
                                Content = "I love this book",
                            }
                        }
                }
            };
        }

        internal async Task<ActionResult<List<Review>>>? GetReviewsByBookId(string bookId)
        {
            if (!reviews[bookId].Any())
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            } 
            else
            {
                return await Task.Run(() => reviews[bookId]);
            }
        }

        public async Task<Author> GetAuthorByAuthorId(string authorId)
        {
            if (authors.FirstOrDefault(au => au.Id.Equals(authorId)) == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                return await Task.Run(() => authors.FirstOrDefault(au => au.Id.Equals(authorId)));
            } 
        }

        public async Task<List<Book>> GetBooksByAuthorId(string authorId)
        {
            if (!books[authorId].Any())
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else
            {
                return await Task.Run(() => books[authorId]);
            }
        }
    }

}
