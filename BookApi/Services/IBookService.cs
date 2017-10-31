using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApi.Models.DTOModels;
using BookApi.Models.ViewModels;

namespace BookApi.Services
{
    public interface IBookService
    {
        // Books         
        IEnumerable<BookDTO> getBooks(DateTime? loanDate, int? loanDuration);
        BookDTO addBook(NewBook book);
        BookAndLoansDTO getBookById(int Id);
        BookDTO deleteBook(int bookId);
        BookDTO editBook(int bookId, NewBook book);

        // Book To User
        IEnumerable<BookDTO> getBooksOnLoanByUser(int userId);
        bool addBookToUser(int bookId, int userId);
        bool deleteBookFromUser(int bookId, int userId);
        bool editBookUser(int bookId, int userId, UserLoan loan);
        UserLoan getBookUser(int bookId, int userId);
        BookDTO getSingleBookById(int Id);
    }
}