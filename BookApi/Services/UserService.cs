using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApi.Repositories;
using BookApi.Models.DTOModels;
using BookApi.Models.ViewModels;

namespace BookApi.Services
{
    public class UserService : IUserService
    {
        /// <summary>
        /// Connection to repository layer
        /// </summary>
        private readonly IRepositoryService _repo;

        /// <summary>
        /// The class constructor
        /// </summary>
        /// <param name="repo"></param>
        public UserService(IRepositoryService repo)
        {
            _repo = repo;
        }
        
        /// <summary>
        /// Gets all users from repo with loanDate and loanDuration if set
        /// </summary>
        public IEnumerable<CreatedUser> getUser(DateTime? loanDate, int? loanDuration)
        {
            return _repo.getUser(loanDate, loanDuration);
        }
        
        /// <summary>
        /// Calls the repo to add a user
        /// </summary>
        public CreatedUser addUser(CreateUser user)
        {
            return _repo.addUser(user);
        }   

        /// <summary>
        /// Calls the repo to delete a user
        /// </summary>
        public bool deleteUser(int userId)
        {
            return _repo.deleteUser(userId);
        }
                
        /// <summary>
        /// Gets a user by its ID from repo
        /// </summary>
        public UserAndLoansDTO getUserById(int userId)
        {
            return _repo.getUserById(userId);
        }
                
        /// <summary>
        /// Calls the repo to edit a specific user
        /// </summary>
        public CreatedUser editUser(int userId, CreateUser user)
        {
            return _repo.editUser(userId, user);
        }
    }
}