using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApi.Models.DTOModels;
using BookApi.Models.ViewModels;

namespace BookApi.Services
{
    public interface IReviewService
    {
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
        
    }
}