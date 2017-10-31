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
        
        // Users
        public IEnumerable<CreatedUser> getUser(DateTime? loanDate, int? loanDuration)
        {
            var user = _repo.getUser(loanDate, loanDuration);
            return user;
        }
        public CreatedUser addUser(CreateUser user)
        {
           return _repo.addUser(user);
        }
        public bool deleteUser(int userId)
        {
            return _repo.deleteUser(userId);
        }
        public UserAndLoansDTO getUserById(int userId)
        {
            var user = _repo.getUserById(userId);
            return user;
        }
        public CreatedUser editUser(int userId, CreateUser user)
        {
            var returnUser = _repo.editUser(userId, user);
            return returnUser;
        }
    }
}