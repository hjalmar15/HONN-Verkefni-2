using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApi.Models.DTOModels;
using BookApi.Models.ViewModels;

namespace BookApi.Services
{
    public interface IUserService
    {
        // Users
        IEnumerable<CreatedUser> getUser(DateTime? loanDate, int? loanDuration);
        CreatedUser addUser(CreateUser user);
        bool deleteUser(int userId);
        UserAndLoansDTO getUserById(int userId);
        CreatedUser editUser(int userId, CreateUser user);
    }
}