using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApi.Models.DTOModels;
using BookApi.Models.ViewModels;

namespace BookApi.Repositories
{
    public interface IRepositoryService
    {
        // Books 
        IEnumerable<BookDTO> getBooks(DateTime? loanDate, int? loanDuration);
        BookDTO addBook(NewBook book);
        BookAndLoansDTO getBookById(int bookID);
        IEnumerable<LoanDTO> getLoansForBookById(int bookId);
        BookDTO deleteBook(int bookId);
        BookDTO editBook(int bookId, NewBook book);

        // Users
        IEnumerable<CreatedUser> getUser(DateTime? loanDate, int? loanDuration);
        CreatedUser addUser(CreateUser user);
        bool deleteUser(int userId);
        UserAndLoansDTO getUserById(int userId);
        CreatedUser editUser(int userId, CreateUser user);

        // Book To User
        IEnumerable<BookDTO> getBooksOnLoanByUser(int userId);
        UserLoan getBookUser(int bookId, int userId);
        bool addBookToUser(int bookId, int userId);
        bool deleteBookFromUser(int bookId, int userId);
        bool editBookUser(int bookId, int userId, UserLoan loan);

        // User and book Reviews
        IEnumerable<ReviewsDTO> getReviewsById(int userId);
        ReviewsDTO getReviewById(int bookId, int userId);

        // BookReviews
        IEnumerable<ReviewsDTO> getBookReview();
        IEnumerable<ReviewsDTO> getBookReviewById(int bookId);
        ReviewsDTO getBookReviewFromUser(int bookId, int userId);
        ReviewsDTO addBookReviewFromUser(int bookId, int userId, ReviewViewModel review);
        ReviewsDTO editBookReviewFromUser(int bookId, int userId, ReviewViewModel review);
        ReviewsDTO deleteReviewFromUser(int bookID, int userId);

        BookDTO getSingleBookById(int Id);
        int getBookCount(); 

    }
}
