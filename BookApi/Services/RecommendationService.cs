using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApi.Models.DTOModels;
using BookApi.Repositories;

namespace BookApi.Services
{
    public class RecommendationService : IRecommendationService
    {
        /// <summary>
        /// Connection to repository layer
        /// </summary>
        private readonly IRepositoryService _repo;

        private const int NumberOfBookRecommendations = 5;

        private IEnumerable<BookDTO> readBooks = null;

        /// <summary>
        /// The class constructor
        /// </summary>
        /// <param name="repo"></param>
        public RecommendationService(IRepositoryService repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Gets a list of book recommendations for a user
        /// </summary>
        /// <param name="user_id"></param>
        public IEnumerable<BookDTO> getRecommendations(int user_id)
        {   
            readBooks = _repo.getBooksOnLoanByUser(user_id);
            int bookRecCount = 1;
            int nrOfBooks = _repo.getBookCount();

            List<BookDTO> recommendationsList = new List<BookDTO>();

            //Cutoff for the unlikely event that a user has already read all books 
            int bookReadCount = 0;

            do{
                //get random number 
                Random r = new Random();
                int randomBookId = r.Next(1, nrOfBooks);

                if(hasReadBook(randomBookId))
                {
                    bookReadCount++;
                }
                else
                {
                    if(!recommendationsList.Contains(_repo.getSingleBookById(randomBookId)))
                    {
                        recommendationsList.Add(_repo.getSingleBookById(randomBookId));
                        bookRecCount++;
                    }

                }
                
            }while(bookRecCount <= NumberOfBookRecommendations || bookReadCount > 200);

            return recommendationsList;
        }

        /// <summary>
        /// Checks if a user has read a given book
        /// </summary>
        /// <param name="user_id"></param>
        private bool hasReadBook(int randomBookId)
        {
            if(readBooks.Contains(_repo.getSingleBookById(randomBookId)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}