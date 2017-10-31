using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApi.Models.DTOModels;
using BookApi.Models.ViewModels;
using BookApi.Repositories;

namespace BookApi.Services
{
    public class ReviewService : IReviewService
    {
        /// <summary>
        /// Connection to repository layer
        /// </summary>
        private readonly IRepositoryService _repo;

        /// <summary>
        /// The class constructor
        /// </summary>
        /// <param name="repo"></param>
        public ReviewService(IRepositoryService repo)
        {
            _repo = repo;
        }

        // User and book Reviews
        public IEnumerable<ReviewsDTO> getReviewsById(int userId)
        {
            var review = _repo.getReviewsById(userId);
            return review;
        }
        public ReviewsDTO getReviewById(int bookId, int userId)
        {
            var review = _repo.getReviewById(bookId, userId);
            return review;
        }

        // BookReviews
        public IEnumerable<ReviewsDTO> getBookReview()
        {
            var Reviews =_repo.getBookReview();
            return Reviews;
        }
        public IEnumerable<ReviewsDTO> getBookReviewById(int bookId)
        {
            var Review = _repo.getBookReviewById(bookId);
            return Review;
        }
        
        public ReviewsDTO getBookReviewFromUser(int bookId, int userId)
        {
            var Review = _repo.getBookReviewFromUser(bookId, userId);
            return Review;
        }
        public ReviewsDTO addBookReviewFromUser(int bookId, int userId, ReviewViewModel review)
        {
            var addedReview = _repo.addBookReviewFromUser(bookId, userId, review);
            return addedReview;
        }
        public ReviewsDTO editBookReviewFromUser(int bookId, int userId, ReviewViewModel review)
        {
            var editReview = _repo.editBookReviewFromUser(bookId, userId, review);
            return editReview;
        }
        public ReviewsDTO deleteReviewFromUser(int bookID, int userId)
        {
            var RemoveReview = _repo.deleteReviewFromUser(bookID, userId);
            return RemoveReview;

        }
    }
}