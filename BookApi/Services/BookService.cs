using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApi.Models.DTOModels;
using BookApi.Models.ViewModels;
using BookApi.Repositories;

namespace BookApi.Services
{
    public class BookService : IBookService
    {
        /// <summary>
        /// Connection to repository layer
        /// </summary>
        private readonly IRepositoryService _repo;

        /// <summary>
        /// The class constructor
        /// </summary>
        /// <param name="repo"></param>
        public BookService(IRepositoryService repo)
        {
            _repo = repo;
        }
                                
        /// <summary>
        /// Gets all books from repo with loanDate and loanDuration if specified
        /// </summary>
        public IEnumerable<BookDTO> getBooks(DateTime? loanDate, int? loanDuration)
        {
            return _repo.getBooks(loanDate, loanDuration);
        }
        
        /// <summary>
        /// Calls the repo to add a new book
        /// </summary>
        public BookDTO addBook(NewBook book)
        {
            return _repo.addBook(book);
        }
                
        /// <summary>
        /// Gets a specific book from repo
        /// </summary>
        public BookAndLoansDTO getBookById(int Id)
        {
            return _repo.getBookById(Id);
        }
                
        /// <summary>
        /// Calls the repo to delete a book
        /// </summary>
        public BookDTO deleteBook(int bookId)
        {
            return _repo.deleteBook(bookId);
        }

        /// <summary>
        /// Calls the repo to edit a book
        /// </summary>
        public BookDTO editBook(int bookId, NewBook book)
        {
            var exists = _repo.getBookById(bookId);

            if (exists == null)
            {
                return null; 
            }
            return _repo.editBook(bookId, book);
        }
        
        /// <summary>
        /// Gets all books on loan for a specific user
        /// </summary>
        public IEnumerable<BookDTO> getBooksOnLoanByUser(int userId)
        {
            return _repo.getBooksOnLoanByUser(userId);
        }

        /// <summary>
        /// Calls repo to add a book to loan for a user
        /// </summary>
        public bool addBookToUser(int bookId, int userId)
        {
           return _repo.addBookToUser(bookId, userId);
        }
        
        /// <summary>
        /// Calls repo to delete a book on loan for a user (Return a book)
        /// </summary>
        public bool deleteBookFromUser(int bookId, int userId)
        {
            return _repo.deleteBookFromUser(bookId, userId);
        }
        
        /// <summary>
        /// Calls repo to edit a book on loan for a user
        /// </summary>
        public bool editBookUser(int bookId, int userId, UserLoan loan)
        {
            return _repo.editBookUser(bookId, userId, loan);
        }

        /// <summary>
        /// Gets a single book by ID
        /// </summary>
        public BookDTO getSingleBookById(int Id)
        {
            return _repo.getSingleBookById(Id);
        }

        /// <summary>
        /// Gets a specific book loan for a specific user
        /// </summary>
        public UserLoan getBookUser(int bookId, int userId)
        {
            return _repo.getBookUser(bookId, userId);
        }

    }
}