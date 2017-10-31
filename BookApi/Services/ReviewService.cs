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
        
        /// <summary>
        /// Gets all reviews by a user from repo
        /// </summary>
        public IEnumerable<ReviewsDTO> getReviewsById(int userId)
        {
            return _repo.getReviewsById(userId);
        }
                
        /// <summary>
        /// Gets a review from a user on a specific book from repo
        /// </summary>
        public ReviewsDTO getReviewById(int bookId, int userId)
        {
            return _repo.getReviewById(bookId, userId);
        }
                
        /// <summary>
        /// Gets all book reviews from repo
        /// </summary>
        public IEnumerable<ReviewsDTO> getBookReview()
        {
            return _repo.getBookReview();
        }
                        
        /// <summary>
        /// Gets all review on a specific book from repo
        /// </summary>
        public IEnumerable<ReviewsDTO> getBookReviewById(int bookId)
        {
            return _repo.getBookReviewById(bookId);
        }
                        
        /// <summary>
        /// Gets a review from a user on a specific book from repo
        /// </summary>
        public ReviewsDTO getBookReviewFromUser(int bookId, int userId)
        {
            return _repo.getBookReviewFromUser(bookId, userId);
        }
                        
        /// <summary>
        /// Calls repo to add a review on a specific book for a user
        /// </summary>
        public ReviewsDTO addBookReviewFromUser(int bookId, int userId, ReviewViewModel review)
        {
            return _repo.addBookReviewFromUser(bookId, userId, review);
        }
                        
        /// <summary>
        /// Calls repo to edit a review on a specific book for a user
        /// </summary>
        public ReviewsDTO editBookReviewFromUser(int bookId, int userId, ReviewViewModel review)
        {
            return _repo.editBookReviewFromUser(bookId, userId, review);;
        }
                                
        /// <summary>
        /// Calls repo to delete a review on a specific book from a user
        /// </summary>
        public ReviewsDTO deleteReviewFromUser(int bookID, int userId)
        {
            return _repo.deleteReviewFromUser(bookID, userId);
        }
    }
}