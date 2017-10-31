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
        // Books 
        public IEnumerable<BookDTO> getBooks(DateTime? loanDate, int? loanDuration)
        {
            var books = _repo.getBooks(loanDate, loanDuration);

            return books;
        }
        public BookDTO addBook(NewBook book)
        {
            var bookCreated = _repo.addBook(book);

            return bookCreated;
        }
        public BookAndLoansDTO getBookById(int Id)
        {
            var book = _repo.getBookById(Id);

            return book;
        }

        public BookDTO deleteBook(int bookId)
        {
            var success = _repo.deleteBook(bookId);

            return success;
        }

        public BookDTO editBook(int bookId, NewBook book)
        {
            var exists = _repo.getBookById(bookId);

            if (exists == null)
            {
                return null; 
            }
            var result = _repo.editBook(bookId, book);
            return result;
        }
        
        // Book To User
        public IEnumerable<BookDTO> getBooksOnLoanByUser(int userId)
        {
            var loans = _repo.getBooksOnLoanByUser(userId);

            return loans;
        }



        public bool addBookToUser(int bookId, int userId)
        {
           return _repo.addBookToUser(bookId, userId);
        }
        public bool deleteBookFromUser(int bookId, int userId)
        {
            return _repo.deleteBookFromUser(bookId, userId);
        }
        public bool editBookUser(int bookId, int userId, UserLoan loan)
        {
            bool success = _repo.editBookUser(bookId, userId, loan);

            return success;
        }

        public BookDTO getSingleBookById(int Id)
        {
            var book = _repo.getSingleBookById(Id);
 
            return book;
        }
        public UserLoan getBookUser(int bookId, int userId)
        {
            var loan = _repo.getBookUser(bookId, userId);
            return loan;
        }

    }
}