

using System;
using System.Collections.Generic;
using BookApi.Models.DTOModels;
using BookApi.Models.ViewModels;
using BookApi.Repositories;
using BookApi.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BookApi.Tests
{
    [TestClass]
    public class BookTest
    {
        private Mock<IRepositoryService> _mockRepo;
        private IEnumerable<BookDTO> _Books;
        private BookDTO _bookDTO;
        
        ///<summary>
        /// Initilizes the list and sets the MockRepo
        ///</summary>
        [TestInitialize]
        public void BookTestSetup()
        {
            var _BooksDTO = new List<BookDTO>();
            _BooksDTO.Add(new BookDTO{
                Title = "This is Glory OF Jon",
                Author = "Jon Jonsson",
                DatePublished = "05/06/2015",
                ISBN = "03123113"
            });
           _BooksDTO.Add(new BookDTO{
                Title = "This is Glory of Arnar",
                Author = "Arnar Joh",
                DatePublished = "05/06/2013",
                ISBN = "111111"
            });

           _BooksDTO.Add(new BookDTO{
                Title = "This is Glory of Diego",
                Author = "Diego",
                DatePublished = "05/06/2015",
                ISBN = "111111"
            });
            _bookDTO = new BookDTO{
                Title = "This is great",
                Author = "Jon Jonsson",
                DatePublished = "03/05/2016",
                ISBN = "543341123"
            };
            _Books = _BooksDTO as IEnumerable<BookDTO>;
            _mockRepo = new Mock<IRepositoryService>();
        }

        ///<summary>
        /// Gets all books
        ///</summary>
        [TestMethod]
        public void getBooks()
        {
            DateTime date = new DateTime();
            _mockRepo.Setup(x => x.getBooks(date, 1)).Returns(_Books);
            var bookServices = new BookService(_mockRepo.Object);
            var bookCount = bookServices.getBooks(date,1);
            bookServices.Should().NotBeNull();
            bookCount.Should().HaveCount(3);  

        }
        
        ///<summary>
        /// Posts a book to the list
        ///</summary>
        [TestMethod]
        public void addBook()
        {
            var _newBook = new NewBook{
                Title = "This is great",
                Author = "Jon Jonsson",
                DatePublished = "03/05/2016",
                ISBN = "543341123"
            };
            var _bookDTO = new BookDTO{
                Title = "This is great",
                Author = "Jon Jonsson",
                DatePublished = "03/05/2016",
                ISBN = "543341123"
            };
             _mockRepo.Setup(x => x.addBook(_newBook)).Returns(_bookDTO);
            var bookServices = new BookService(_mockRepo.Object);
            var bookCount = bookServices.addBook(_newBook);
            bookServices.Should().NotBeNull();
            bookCount.Title.Should().BeEquivalentTo("This is great");
            bookCount.Author.Should().BeEquivalentTo("Jon Jonsson");
            bookCount.Should().NotBeNull();

            
        }
        
        ///<summary>
        /// Gets book by its ID
        ///</summary>
        [TestMethod]
        public void getBookById()
        {
            var LoanHistory = new List<LoanDTO>();
            LoanHistory.Add(new LoanDTO{
                Loanee = new CreatedUser{
                    Name = "Johnny",
                    Address = "SomeCoolAddress",
                    Email = "JonJonsson@gmail.com",
                    PhoneNumber = "55447799"
                }
                
            });
            var _newBookAndLoans = new BookAndLoansDTO{
                Id = 1,
                Title = "The glory of arnar",
                Author = "Arnar Joh",
                DatePublished = "05/06/2016",
                ISBN = "11222333",
                LoanHistory = LoanHistory
            };
            
            _mockRepo.Setup(x => x.getBookById(1)).Returns(_newBookAndLoans);
            var bookServices = new BookService(_mockRepo.Object);
            var bookCount = bookServices.getBookById(1);
            bookServices.Should().NotBeNull();
            bookCount.Title.Should().BeEquivalentTo("The glory of arnar");
            bookCount.Author.Should().BeEquivalentTo("Arnar Joh");
            bookCount.Should().NotBeNull();

            
        }
        
        ///<summary>
        /// Deletes a book from the list
        ///</summary>
        [TestMethod]
        public void deleteBook()
        {
            _mockRepo.Setup(x => x.deleteBook(1)).Returns(_bookDTO);
            var bookServices = new BookService(_mockRepo.Object);
            var bookCount = bookServices.deleteBook(1);
            bookServices.Should().NotBeNull();
            bookCount.Title.Should().BeEquivalentTo("This is great");
            bookCount.Author.Should().BeEquivalentTo("Jon Jonsson");
            bookCount.Should().NotBeNull();
        }

        ///<summary>
        /// Edits a book from the list
        ///</summary>
        [TestMethod]
        public void editBook()
        {
            var newBook = new NewBook{
                Title = "SomeTitle",
                Author = "SomeAuthor",
                DatePublished = "05/01/2016",
                ISBN = "111122223333"
            };

            _mockRepo.Setup(x => x.editBook(1,newBook)).Returns(_bookDTO);
            var bookServices = new BookService(_mockRepo.Object);
            var bookCount = bookServices.editBook(1,newBook);
            bookServices.Should().NotBeNull();
            bookCount.Should().BeNull();

        }
        
        ///<summary>
        /// Adds a book to a user
        ///</summary>
        [TestMethod]
        public void addBookToUser()
        {
            _mockRepo.Setup(x => x.addBookToUser(1,1));
            var bookServices = new BookService(_mockRepo.Object);
            bookServices.Should().NotBeNull();
    

        }

        ///<summary>
        /// Deletes a book from User (User returns the book)
        ///</summary>
        [TestMethod]
        public void deleteBookFromUser()
        {
            _mockRepo.Setup(x => x.deleteBookFromUser(1,1));
            var bookServices = new BookService(_mockRepo.Object);
            bookServices.Should().NotBeNull();

        }
        
        ///<summary>
        /// Edits the dates of when the user got the book and returned it
        ///</summary>
        [TestMethod]
        public void editBookUser()
        {
            var userLoan = new UserLoan{
                LoanDate = "03/01/2016",
                ReturnedDate = "05/02/2017"
            };
            _mockRepo.Setup(x => x.editBookUser(1,1,userLoan));
            var bookServices = new BookService(_mockRepo.Object);
            bookServices.Should().NotBeNull(); 

        }

        ///<summary>
        /// Gets a loan from book that is loaned from a user
        ///</summary>
        [TestMethod]
        public void getBookUser()
        {
            var userLoan = new UserLoan{
                LoanDate = "03/01/2016",
                ReturnedDate = "05/02/2017"
            };
            _mockRepo.Setup(x => x.getBookUser(1,1)).Returns(userLoan);
            var bookServices = new BookService(_mockRepo.Object);
            var bookCount = bookServices.getBookUser(1,1);
            bookServices.Should().NotBeNull(); 
            bookCount.LoanDate.Should().BeEquivalentTo("03/01/2016");
            bookCount.ReturnedDate.Should().BeEquivalentTo("05/02/2017");
        }
        
        ///<summary>
        /// Gets a single book by its ID
        ///</summary>
        [TestMethod]
        public void getSingleBookById()
        {
            _mockRepo.Setup(x => x.getSingleBookById(1)).Returns(_bookDTO);
            var bookServices = new BookService(_mockRepo.Object);
            var bookCount = bookServices.getSingleBookById(1);
            bookServices.Should().NotBeNull();
            bookCount.Title.Should().BeEquivalentTo("This is great");
            bookCount.Author.Should().BeEquivalentTo("Jon Jonsson");
            bookCount.Should().NotBeNull();
        }
    }
}