using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApi.Models.DTOModels;
using BookApi.Models.Entities;
using BookApi.Models.ViewModels;

namespace BookApi.Repositories
{
    public class RepositoryServices : IRepositoryService
    {

        /// <summary>
        /// Database connection
        /// </summary>
        private readonly AppDataContext _db;

        /// <summary>
        /// The class constructor
        /// </summary>
        /// <param name="db"></param>
        public RepositoryServices(AppDataContext db)
        {
            _db = db;
        }




        /// <summary>
        /// Get all books from database that 
        /// and if queryparameter are not null
        /// checks loan durations and loandate.
        /// <param name = "loanDate"></param>
        /// <param name = "loanDate"></param>
        /// <returns type = "IEnumerable<BookDTO>"> </returns>
        /// </summary>
		public IEnumerable<BookDTO> getBooks(DateTime? loanDate, int? loanDuration)
        {
            DateTime LoanDate = Convert.ToDateTime(loanDate);

            if(loanDate == null && loanDuration == null){
                var books = (from b in _db.Books 
                                select new BookDTO
                                {
                                    Id = b.Id,
                                    Title = b.Title,
                                    Author = b.Author,
                                    DatePublished = b.DatePublished,
                                    ISBN = b.ISBN
                                }).ToList();
                return books;
            }
            else if(loanDate != null && loanDuration == null)
            {
                //Done
                var book = (from b in _db.Books join ub in _db.UserBooks on b.Id equals ub.BookId
                        where ub.LoanDate.Ticks <= LoanDate.Ticks && (ub.ReturnedDate.Ticks == 0 || ub.ReturnedDate.Ticks > LoanDate.Ticks)
                        select new BookDTO
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            DatePublished = b.DatePublished,
                            ISBN = b.ISBN
                        }).ToList();
                return book;
            }
            else if(loanDate == null && loanDuration != null)
            {
                //Done
                var book = (from b in _db.Books join ub in _db.UserBooks on b.Id equals ub.BookId
                        where (ub.ReturnedDate.Ticks != 0 && (loanDuration <= (ub.ReturnedDate - ub.LoanDate).TotalDays)) || (ub.ReturnedDate.Ticks == 0 && (loanDuration <= (DateTime.Now - ub.LoanDate).TotalDays))
                        select new BookDTO
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            DatePublished = b.DatePublished,
                            ISBN = b.ISBN
                        }).ToList();
                return book;
            }
            else
            {
                //Done
                var book = (from b in _db.Books join ub in _db.UserBooks on b.Id equals ub.BookId
                        where ((ub.LoanDate.Ticks <= LoanDate.Ticks && (ub.ReturnedDate.Ticks == 0 || ub.ReturnedDate.Ticks > LoanDate.Ticks)) &&
                            (ub.ReturnedDate.Ticks != 0 && (loanDuration <= (ub.ReturnedDate - ub.LoanDate).TotalDays)) || (ub.ReturnedDate.Ticks == 0 && (loanDuration <= (DateTime.Now - ub.LoanDate).TotalDays))) 
                        select new BookDTO
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            DatePublished = b.DatePublished,
                            ISBN = b.ISBN
                        }).ToList();
                return book;
            }
        }

        /// <summary>
        /// add book to database 
        /// <param name = "book"></param>
        /// <returns type = "BookDTO"> </returns>
        /// </summary>
        public BookDTO addBook(NewBook book)
        {
            Book newEntityBook = new Book {
                Title = book.Title,
                Author = book.Author,
                DatePublished = book.DatePublished,
                ISBN = book.ISBN
            };

            _db.Books.Add(newEntityBook);
            _db.SaveChanges();

            BookDTO bookCreated = new BookDTO{
                Id = (from b in _db.Books orderby b.Id descending
                        select b.Id).FirstOrDefault(),   
                Title = book.Title,
                Author = book.Author,
                DatePublished = book.DatePublished,
                ISBN = book.ISBN
            };

            return bookCreated;
        }

        /// <summary>
        /// Returns a book from datbase from specific Id
        /// if not found returns null.
        /// <param name = "bookID"></param>
        /// <returns type = "BookAndLoansDTO"> </returns>
        /// </summary>
        public BookAndLoansDTO getBookById(int bookID)
        {
            var book = (from b in _db.Books 
                            where b.Id == bookID
                            select new BookAndLoansDTO
                            {
                                Id = b.Id,
                                Title = b.Title,
                                Author = b.Author,
                                DatePublished = b.DatePublished,
                                ISBN = b.ISBN
                }).SingleOrDefault();

                if(book != null)
                {
                    book.LoanHistory = getLoansForBookById(bookID);
                }

            return book;
        }


        /// <summary>
        /// Get all loans from book by Id
        /// <param name = "bookID"></param>
        /// <returns type = "IEnumerable<LoanDTO>"> </returns>
        /// </summary>
        public IEnumerable<LoanDTO> getLoansForBookById(int bookId)
        {
            var loans = (from ub in _db.UserBooks
                            where ub.BookId == bookId
                            join u in _db.Users on ub.UserId equals u.Id
                select new LoanDTO
                {
                    Loanee = new CreatedUser
                        {
                            Id = u.Id,
                            Name = u.Name,
                            Address = u.Address,
                            Email = u.Email,
                            PhoneNumber = u.PhoneNumber
                        },
                    LoanDate = ub.LoanDate,
                    ReturnedDate = ub.ReturnedDate
                }).ToList();

            return loans;
        }
       

        /// <summary>
        /// Deletes a single book from database
        /// <param name = "bookID"></param>
        /// <returns type = "BookDTO"> </returns>
        /// </summary>
        public BookDTO deleteBook(int bookId)
        {
            var toDelete = (from b in _db.Books
                where b.Id == bookId
                select b).SingleOrDefault();

            var book = new BookDTO{
                Title = toDelete.Title,
                Author = toDelete.Author,
                DatePublished = toDelete.DatePublished,
                ISBN = toDelete.ISBN
            };

            if(toDelete != null)
            {
                _db.Books.Remove(toDelete);
                _db.SaveChanges();
                return book;
            }
            return null;
        }


        /// <summary>
        ///  Edits a single book from database.
        /// <param name = "bookID"></param>
        /// <param name = "book"></param>
        /// <returns type = "BookDTO"> </returns>
        /// </summary>
        public BookDTO editBook(int bookId, NewBook book)
        {
            var result = (from b in _db.Books
              where b.Id == bookId
              select b).SingleOrDefault();
            
            var _bookDTO = new BookDTO{
                Title = result.Title,
                Author = result.Author,
                DatePublished = result.DatePublished,
                ISBN = result.ISBN
            };

            result.Title = book.Title;
            result.Author = book.Author;
            result.DatePublished = book.DatePublished;
            result.ISBN = book.ISBN;

            if (result != null)
            {
                _db.SaveChanges();
                return _bookDTO;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///  Gets users from database and if loandate and 
        /// loanduration is not null then finds all users 
        /// that fullfills the rquirements.
        /// <param name = "loanDate"></param>
        /// <param name = "loanDuration"></param>
        /// <returns type = "IEnumerable<CreatedUser>"> </returns>
        /// </summary>
        public IEnumerable<CreatedUser> getUser(DateTime? loanDate, int? loanDuration)
        {
            DateTime LoanDate = Convert.ToDateTime(loanDate);

            if(loanDate == null && loanDuration == null){
                var user = (from u in _db.Users 
                        select new CreatedUser
                        {
                            Id = u.Id,
                            Name = u.Name,
                            Address = u.Address,
                            Email = u.Email,
                            PhoneNumber = u.PhoneNumber
                        }).ToList();
                return user;
            }
            else if(loanDate != null && loanDuration == null)
            {
                //Done
                var user = (from u in _db.Users join ub in _db.UserBooks on u.Id equals ub.UserId
                        where ub.LoanDate.Ticks <= LoanDate.Ticks && (ub.ReturnedDate.Ticks == 0 || ub.ReturnedDate.Ticks > LoanDate.Ticks)
                        select new CreatedUser
                        {
                            Id = u.Id,
                            Name = u.Name,
                            Address = u.Address,
                            Email = u.Email,
                            PhoneNumber = u.PhoneNumber
                        }).ToList();
                return user;
            }
            else if(loanDate == null && loanDuration != null)
            {
                //Done
                var user = (from u in _db.Users join ub in _db.UserBooks on u.Id equals ub.UserId
                        where (ub.ReturnedDate.Ticks != 0 && (loanDuration <= (ub.ReturnedDate - ub.LoanDate).TotalDays)) || (ub.ReturnedDate.Ticks == 0 && (loanDuration <= (DateTime.Now - ub.LoanDate).TotalDays))
                        select new CreatedUser
                        {
                            Id = u.Id,
                            Name = u.Name,
                            Address = u.Address,
                            Email = u.Email,
                            PhoneNumber = u.PhoneNumber
                        }).ToList();
                return user;
            }
            else
            {
                //Done
                var user = (from u in _db.Users join ub in _db.UserBooks on u.Id equals ub.UserId
                        where ((ub.LoanDate.Ticks <= LoanDate.Ticks && (ub.ReturnedDate.Ticks == 0 || ub.ReturnedDate.Ticks > LoanDate.Ticks)) &&
                            (ub.ReturnedDate.Ticks != 0 && (loanDuration <= (ub.ReturnedDate - ub.LoanDate).TotalDays)) || (ub.ReturnedDate.Ticks == 0 && (loanDuration <= (DateTime.Now - ub.LoanDate).TotalDays))) 
                        select new CreatedUser
                        {
                            Id = u.Id,
                            Name = u.Name,
                            Address = u.Address,
                            Email = u.Email,
                            PhoneNumber = u.PhoneNumber
                        }).ToList();
                return user;
            }
        }


        /// <summary>
        /// Add user to database 
        /// <param name = "user"></param>
        /// <returns type = "CreatedUser"> </returns>
        /// </summary>
        public CreatedUser addUser(CreateUser user)
        {
            User newUser = new User{
                Name = user.Name,
                Address = user.Address,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            _db.Users.Add(newUser);
            _db.SaveChanges();

            CreatedUser retUser = new CreatedUser{
                Id = (from u in _db.Users orderby u.Id descending
                        select u.Id).FirstOrDefault(),   
                Name = user.Name,
                Address = user.Address,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return retUser;
        }

        /// <summary>
        /// Deletes a single user from database
        /// <param name = "userId"></param>
        /// <returns type = "bool"> </returns>
        /// </summary>
        public bool deleteUser(int userId)
        {
            var toDelete = (from u in _db.Users
                where u.Id == userId
                select u).SingleOrDefault();
                
            if(toDelete != null)
            {
                _db.Users.Remove(toDelete);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a single user from database.
        /// <param name = "userId"></param>
        /// <returns type = "UserAndLoansDTO"> </returns>
        /// </summary>
        public UserAndLoansDTO getUserById(int userId)
        {
            var user = (from u in _db.Users
                where u.Id == userId
                select u).SingleOrDefault();
            
            if(user != null)
            {
                var returnUser = getBooksOnLoanByUser(userId);
                UserAndLoansDTO retUser = new UserAndLoansDTO{
                    Id = user.Id,
                    Name = user.Name,
                    Address = user.Address,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    listOfUserBooks = returnUser
                };

                return retUser;
            }
            return null;
        }

        /// <summary>
        /// Edits a single user from database
        /// <param name = "userId"></param>
        /// <param name = "user"></param>
        /// <returns type = "CreatedUser"> </returns>
        /// </summary>
        public CreatedUser editUser(int userId, CreateUser user)
        {
            CreatedUser updatedUser = new CreatedUser{
                Id = userId,
                Name = user.Name,
                Address = user.Address,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            var result = (from u in _db.Users
              where u.Id == updatedUser.Id
              select u).SingleOrDefault();

            result.Name = updatedUser.Name;
            result.Address = updatedUser.Address;
            result.Email = updatedUser.Email;
            result.PhoneNumber = updatedUser.PhoneNumber;

            if (result != null)
            {
                _db.SaveChanges();
                CreatedUser retUser = new CreatedUser{
                    Id = result.Id,
                    Name = result.Name,
                    Address = result.Address,
                    Email = result.Email,
                    PhoneNumber = result.PhoneNumber
                };
                return retUser;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Get books that are loaned
        /// <param name = "userId"></param>
        /// <returns type = "IEnumerable<BookDTO>"> </returns>
        /// </summary>
        public IEnumerable<BookDTO> getBooksOnLoanByUser(int userId)
        {
            var date = Convert.ToDateTime(null);

            var loans = (from ub in _db.UserBooks
                        where ub.UserId == userId && ub.ReturnedDate == date
                        join b in _db.Books on ub.BookId equals b.Id
                    select new BookDTO
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Author = b.Author,
                        DatePublished = b.DatePublished,
                        ISBN = b.ISBN
                    }).ToList();

            return loans;
        }


        /// <summary>
        ///  Add book to user 
        /// <param name = "bookId"></param>
        /// <param name = "userId"></param>
        /// <returns type = "bool"> </returns>
        /// </summary>
        public bool addBookToUser(int bookId, int userId)
        {
            bool doesExist = false;
            var exists = (from ub in _db.UserBooks 
                            where ub.UserId == userId && ub.BookId == bookId && ub.ReturnedDate < DateTime.Now 
                            select ub);
            if(exists.Count() > 0)
            {
                doesExist = true;
            }
            if(doesExist)
            {
                return false;
            }
            String today = DateTime.Now.ToString("yyyy-MM-dd");

            UserBook newUserBook = new UserBook{
                BookId = bookId,
                UserId = userId,
                LoanDate = Convert.ToDateTime(today)
            };

            _db.UserBooks.Add(newUserBook);
            _db.SaveChanges();
            return true;
        }

        /// <summary>
        ///  Delets a book that user has loaned.
        /// <param name = "bookId"></param>
        /// <param name = "userId"></param>
        /// <returns type = "bool"> </returns>
        /// </summary>
        public bool deleteBookFromUser(int bookId, int userId)
        {
            bool doesExist = false;
            var exists = (from ub in _db.UserBooks 
                            where ub.UserId == userId && ub.BookId == bookId && ub.ReturnedDate == DateTime.MinValue
                            select ub);
            if(exists.Count() > 0)
            {
                doesExist = true;
            }
            if(doesExist == false)
            {
                return false;
            }
            String today = DateTime.Now.ToString("yyyy-MM-dd");

            var returnUserBook = (from ub in _db.UserBooks
                                where ub.BookId == bookId && ub.UserId == userId 
                                select ub).SingleOrDefault();

            returnUserBook.ReturnedDate = Convert.ToDateTime(today);

            _db.SaveChanges();
            return true;
        }


        /// <summary>
        ///  Edit book that user has loaned
        /// <param name = "bookId"></param>
        /// <param name = "userId"></param>
        /// <param name = "loan"></param>
        /// <returns type = "bool"> </returns>
        /// </summary>
        public bool editBookUser(int bookId, int userId, UserLoan loan)
        {
            var toUpdate = (from ub in _db.UserBooks
                         where ub.BookId == bookId && ub.UserId == userId
                         select ub).SingleOrDefault();
 
            if(loan.LoanDate == "0" || loan.LoanDate == "" || loan.LoanDate == null)
            {
                toUpdate.LoanDate = Convert.ToDateTime(null);
            }
            else
            {
                toUpdate.LoanDate = Convert.ToDateTime(loan.LoanDate);
            }

            if(loan.ReturnedDate == "0" || loan.ReturnedDate == "" || loan.ReturnedDate == null)
            {
                toUpdate.ReturnedDate = Convert.ToDateTime(null);
            }
            else
            {
                toUpdate.ReturnedDate = Convert.ToDateTime(loan.ReturnedDate);
            }
            

            if (toUpdate != null)
            {
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///  Gets all reviews from userid that he has posed 
        /// to any book.
        /// <param name = "userId"></param>
        /// <returns type = "IEnumerable<ReviewsDTO>"> </returns>
        /// </summary>
        public IEnumerable<ReviewsDTO> getReviewsById(int userId)
        {
            var reviews = (from r in _db.Reviews join u in _db.Users on r.UserId equals u.Id
                            join b in _db.Books on r.BookId equals b.Id 
                            where userId == r.UserId
                            select new ReviewsDTO{
                                BookName = b.Title,
                                UserName = u.Name,
                                Stars = r.Stars,
                                Review = r.UserReview
                            }).ToList();
            return reviews;
        }

        /// <summary>
        ///  Gets a single review by bookId and userID
        /// <param name = "bookId"></param>
        /// <param name = "userId"></param>
        /// <returns type = "ReviewsDTO"> </returns>
        /// </summary>
        public ReviewsDTO getReviewById(int bookId, int userId)
        {
            var review = (from r in _db.Reviews join u in _db.Users on r.UserId equals u.Id
                            join b in _db.Books on r.BookId equals b.Id 
                            where r.BookId == bookId && r.UserId == userId
                            select new ReviewsDTO{
                                BookName = b.Title,
                                UserName = u.Name,
                                Stars = r.Stars,
                                Review = r.UserReview
                            }).SingleOrDefault();
            if(review == null)
            {
                return null;
            }
            return review;
        }

        /// <summary>
        ///  Get all book reviews that exists in database.
        /// <returns type = "IEnumerable<ReviewsDTO>"> </returns>
        /// </summary>
        public IEnumerable<ReviewsDTO> getBookReview()
        {
            var ReviewExists = _db.Reviews.FirstOrDefault();
            if (ReviewExists == null)
            {
                return null;
            }
            var Reviews = from c in _db.Reviews 
                            join b in _db.Books on c.BookId equals b.Id
                            join u in _db.Users on c.UserId equals u.Id
                            select new ReviewsDTO{
                                BookName = b.Title,
                                UserName = u.Name,
                                Stars = c.Stars,
                                Review = c.UserReview
                            };
            return Reviews;
        }

        /// <summary>
        ///  Get all book review that users have posed.
        /// <param name = "bookId"></param>
        /// <returns type = "IEnumerable<ReviewsDTO>"> </returns>
        /// </summary>
        public IEnumerable<ReviewsDTO> getBookReviewById(int bookId)
        {
            var ReviewExists = (from r in _db.Reviews where r.BookId == bookId
                                select r);
            if (ReviewExists == null)
            {
                return null;
            } 
            var Reviews = (from r in _db.Reviews where r.BookId == bookId
                            join b in _db.Books on r.BookId equals b.Id
                            join u in _db.Users on r.UserId equals u.Id
                            select new ReviewsDTO{
                                BookName = b.Title,
                                UserName = u.Name,
                                Stars = r.Stars,
                                Review = r.UserReview
                            });
            return Reviews;
        }

        /// <summary>
        ///  Get a single book review that specific user has posted.
        /// <param name = "bookId"></param>
        /// <param name = "userId"></param>
        /// <returns type = "ReviewsDTO"> </returns>
        /// </summary>
        public ReviewsDTO getBookReviewFromUser(int bookId, int userId)
        {
            var IfReviewExists = (from r in _db.Reviews where r.BookId == bookId 
                                && r.UserId == userId select r);
            if (IfReviewExists == null)
            {
                return null;
            }
            var userName = (from u in _db.Users where u.Id == userId select u.Name).SingleOrDefault();
            var Review = (from r in _db.Reviews where r.BookId == bookId
                         join b in _db.Books on r.BookId equals b.Id
                         where r.UserId == userId
                         select new ReviewsDTO{
                             BookName = b.Title,
                             UserName = userName,
                             Stars = r.Stars,
                             Review = r.UserReview
                         }).SingleOrDefault();
            return Review;
        }

        /// <summary>
        ///  Post a single review to book from user.
        /// <param name = "bookId"></param>
        /// <param name = "userId"></param>
        /// <param name = "review"></param>
        /// <returns type = "ReviewsDTO"> </returns>
        /// </summary>
        public ReviewsDTO addBookReviewFromUser(int bookId, int userId, ReviewViewModel review)
        {
           var IfReviewExists = (from r in _db.Reviews where r.BookId == bookId 
                                && r.UserId == userId select r).SingleOrDefault();
            if (IfReviewExists != null)
            {
                return null;
            }
            Review newReview = new Review{
                BookId = bookId,
                UserId = userId,
                Stars = review.Stars,
                UserReview = review.UserReview
            };
            _db.Reviews.Add(newReview);
            _db.SaveChanges();

            var returnReview = (from r in _db.Reviews where r.BookId == bookId
                         join b in _db.Books on r.BookId equals b.Id
                         join u in _db.Users on r.UserId equals u.Id
                         select new ReviewsDTO{
                             BookName = b.Title,
                             UserName = u.Name,
                             Stars = r.Stars,
                             Review = r.UserReview
                         }).SingleOrDefault();
            return returnReview;

        }

        
        /// <summary>
        ///  Edit single review from specific user and book.
        /// <param name = "bookId"></param>
        /// <param name = "userId"></param>
        /// <param name = "review"></param>
        /// <returns type = "ReviewsDTO"> </returns>
        /// </summary>
        public ReviewsDTO editBookReviewFromUser(int bookId, int userId, ReviewViewModel review)
        {
           var IfReviewExists = (from r in _db.Reviews where r.BookId == bookId 
                                && r.UserId == userId select r);
            if (IfReviewExists == null)
            {
                return null;
            }
            var userReview = (from r in _db.Reviews where r.BookId == bookId
                                && r.UserId == userId select r).SingleOrDefault();
            
            userReview.Stars = review.Stars;
            userReview.UserReview = review.UserReview;
            _db.SaveChanges();
            var returnReview = new ReviewsDTO{
                Stars = review.Stars,
                Review = review.UserReview
            };
            return returnReview;

        }

         /// <summary>
        ///  Deletes a single review that exists on book from specific user..
        /// <param name = "bookId"></param>
        /// <param name = "userId"></param>
        /// <returns type = "ReviewsDTO"> </returns>
        /// </summary>
        public ReviewsDTO deleteReviewFromUser(int bookID, int userId)
        {
            var IfReviewExists = (from r in _db.Reviews where r.BookId == bookID 
                                && r.UserId == userId select r).SingleOrDefault();
            if (IfReviewExists == null)
            {
                return null;
            }
            _db.Reviews.Remove(IfReviewExists);
            _db.SaveChanges();
            return new ReviewsDTO{
                Stars = IfReviewExists.Stars,
                Review = IfReviewExists.UserReview
            };
        }

        /// <summary>
        ///  Get single book by Id (Helper function)
        /// <param name = "Id"></param>
        /// <returns type = "BookDTO"> </returns>
        /// </summary>
        public BookDTO getSingleBookById(int Id)
        {
            var book = (from b in _db.Books 
                            where b.Id == Id
                            select new BookDTO
                            {
                                Id = b.Id,
                                Title = b.Title,
                                Author = b.Author,
                                DatePublished = b.DatePublished,
                                ISBN = b.ISBN
                }).SingleOrDefault();
            
            return book;
        }

        /// <summary>
        ///  Get book count from database (Helper function)
        /// <returns type = "int"> </returns>
        /// </summary>
        public int getBookCount()
        {
            var bookListCount = (from b in _db.Books
                        select b).Count();
                        
            return bookListCount;
        }

        /// <summary>
        ///  get book that user has loaned (Helper function)
        /// <param name = "bookId"></param>
        /// <param name = "userId"></param>
        /// <returns type = "int"> </returns>
        /// </summary>
        public UserLoan getBookUser(int bookId, int userId)
        {
            var loan = (from ub in _db.UserBooks
                        where ub.BookId == bookId && ub.UserId == userId
                        select ub).SingleOrDefault();
             
            if(loan != null)
            {
                UserLoan retLoan = new UserLoan{
                    LoanDate = Convert.ToString(loan.LoanDate),
                    ReturnedDate = Convert.ToString(loan.ReturnedDate)
                };

                return retLoan;
            }
            return null;
        }

    }
}
