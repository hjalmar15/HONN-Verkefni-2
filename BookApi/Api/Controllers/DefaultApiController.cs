using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookApi.Services;

using Newtonsoft.Json;
using BookApi.Models.DTOModels;
using BookApi.Models.ViewModels;

namespace BookApi.Api.Controllers
{ 
    /// <summary>
    /// Post returns 201 and the newly created item
    /// Put returns 200 and the newly updated item
    /// Get returns 200 and the item asked for
    /// Delete returns 200 and the string "(item) deleted"
    /// </summary>
    public class DefaultApiController : Controller
    { 


        /// <summary>
        /// Connection to service layer
        /// </summary>
        private readonly IBookService _BookService;
        private readonly IRecommendationService _RecommendationService;
        private readonly IReviewService _ReviewService;
        private readonly IUserService _UserService;

        /// <summary>
        /// The class constructor
        /// </summary>
        /// <param name="coursesService"></param>
        public DefaultApiController(IBookService bookService, IRecommendationService recommendationService, IReviewService reviewService,
                                    IUserService userService)
        {
            _BookService = bookService;
            _RecommendationService = recommendationService;
            _ReviewService = reviewService;
            _UserService = userService;
        }

    #region "Books"

        /// <summary>
        /// Gets some books
        /// </summary>
        /// <remarks>Returns a list containing all books</remarks>
        /// <response code="200">A list of books</response>
        [HttpGet]
        [Route("/api/v1/books")]
        public virtual IActionResult BooksGet([FromQuery] string loanDate, [FromQuery] string loanDuration)
        { 
            DateTime LoanDate;
            int LoanDuration;
            if(loanDate != null)
            {
                LoanDate = Convert.ToDateTime(loanDate);
                if(loanDuration != null)
                {
                    LoanDuration = Int32.Parse(loanDuration);
                    var booksWithBoth = _BookService.getBooks(LoanDate, LoanDuration);
                    return Ok(booksWithBoth);
                }
                var booksWithDate = _BookService.getBooks(LoanDate, null);
                return Ok(booksWithDate);
            } 
            else if(loanDuration != null)
            {
                LoanDuration = Int32.Parse(loanDuration);
                var booksWithDuration = _BookService.getBooks(null, LoanDuration);
                return Ok(booksWithDuration);
            }
            
            var books = _BookService.getBooks(null, null);
            
            return Ok(books);
        }

        /// <summary>
        /// Creates a book
        /// </summary>
        /// <remarks>Adds a new book to the books list.</remarks>
        /// <param name="book">The book to create.</param>
        /// <response code="204">Book succesfully created.</response>
        /// <response code="400">Book couldn&#39;t have been created.</response>
        [HttpPost]
        [Route("/api/v1/books")]
        public virtual IActionResult BooksPost([FromBody]NewBook book)
        { 
            if (!ModelState.IsValid){
                return StatusCode(404);
            }

            var bookCreated = _BookService.addBook(book);

            return StatusCode((int)HttpStatusCode.Created, bookCreated);
        }

        /// <summary>
        /// Gets a book
        /// </summary>
        /// <remarks>Returns a single book for its id.</remarks>
        /// <param name="book_id">The books book_id.</param>
        /// <response code="200">A book</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Unknown error occurred.</response>
        [HttpGet]
        [Route("/api/v1/books/{book_id}")]
        public virtual IActionResult BooksBookIdGet([FromRoute]int book_id)
        { 
            var book = _BookService.getBookById(book_id);

            if (book == null)
            {
                return StatusCode(404);
            }
            
            return Ok(book);
        }

        /// <summary>
        /// Deletes a book
        /// </summary>
        /// <remarks>Delete a single book identified via its id</remarks>
        /// <param name="book_id">The book&#39;s book_id</param>
        /// <response code="204">book successfully deleted.</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Unknown error occurred.</response>
        [HttpDelete]
        [Route("/api/v1/books/{book_id}")]
        public virtual IActionResult BooksBookIdDelete([FromRoute]int book_id)
        { 
            var success = _BookService.deleteBook(book_id);

            if (success != null)
            {
                return StatusCode(200, "Book deleted");
            }
            else
            {
                return StatusCode(404, "Book not found");
            }
        }

        /// <summary>
        /// Changes a book
        /// </summary>
        /// <remarks>Changes a single book for its id.</remarks>
        /// <param name="book_id">The book&#39;s book_id</param>
        /// <param name="book">The book to update.</param>
        /// <response code="200">A book</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Unknown error occurred.</response>
        [HttpPut]
        [Route("/api/v1/books/{book_id}")]
        public virtual IActionResult BooksBookIdPut([FromRoute]int book_id, [FromBody]NewBook book)
        { 
            var toUpdate = _BookService.getBookById(book_id);

            BookDTO result;

            if (toUpdate == null || book == null)
            {
                return StatusCode(404);
            }

            if (book_id == toUpdate.Id)
            {
                if(ModelState.IsValid)
                {
                    result = _BookService.editBook(book_id, book);
                }
                else
                {
                    return StatusCode(404, "Book object invalid");
                }
            }
            else
            {
                return NotFound("BookId is incorrect");
            }

            if(result != null)
            {
                var updatedBook = _BookService.getBookById(book_id);
                return Ok(updatedBook);
            }
            else
            {
                return StatusCode(404);
            }
        }
    #endregion

    #region "Users"

        /// <summary>
        /// Gets some users
        /// </summary>
        /// <remarks>Returns a list containing all users</remarks>
        /// <response code="200">A list of users</response>
        [HttpGet]
        [Route("/api/v1/users")]
        [ActionName("getUser")]
        public virtual IActionResult UsersGet([FromQuery] string loanDate, [FromQuery] string loanDuration)
        { 
            DateTime LoanDate;
            int LoanDuration;
            if(loanDate != null)
            {
                LoanDate = Convert.ToDateTime(loanDate);
                if(loanDuration != null)
                {
                    LoanDuration = Int32.Parse(loanDuration);
                    var usersWithBoth = _UserService.getUser(LoanDate, LoanDuration);
                    return Ok(usersWithBoth);
                }
                var usersWithDate = _UserService.getUser(LoanDate, null);
                return Ok(usersWithDate);
            } 
            else if(loanDuration != null)
            {
                LoanDuration = Int32.Parse(loanDuration);
                var usersWithDuration = _UserService.getUser(null, LoanDuration);
                return Ok(usersWithDuration);
            }
            var users = _UserService.getUser(null, null);
            return Ok(users);
        }


        /// <summary>
        /// Creates a user
        /// </summary>
        /// <remarks>Adds a new user to the users list.</remarks>
        /// <param name="user">The user to create.</param>
        /// <response code="204">User succesfully created.</response>
        /// <response code="400">User couldn&#39;t have been created.</response>
        [HttpPost]
        [Route("/api/v1/users")]
        public virtual IActionResult UsersPost([FromBody]CreateUser user)
        { 
            if(ModelState.IsValid)
            {
                var retUser = _UserService.addUser(user);
                return CreatedAtRoute("created", new { user_id = retUser.Id }, retUser);
            }
            else
            {
                return BadRequest("User must have a name");
            }
            
        }


        /// <summary>
        /// Gets a user
        /// </summary>
        /// <remarks>Returns a single user for its id.</remarks>
        /// <param name="user_id">The person&#39;s user_id</param>
        /// <response code="200">A user</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Unknown error occurred.</response>
        [HttpGet("{user_id}", Name="created")]
        [Route("/api/v1/users/{user_id}")]
        public virtual IActionResult UsersUserIdGet([FromRoute]int user_id)
        { 
            var user = _UserService.getUserById(user_id);
            if(user == null)
            {
                return NoContent();
            }
            return Ok(user);
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <remarks>Delete a single user identified via its id</remarks>
        /// <param name="user_id">The person&#39;s user_id</param>
        /// <response code="204">User successfully deleted.</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Unknown error occurred.</response>
        [HttpDelete]
        [Route("/api/v1/users/{user_id}")]
        public virtual IActionResult UsersUserIdDelete([FromRoute]int user_id)
        { 
            var deleted = _UserService.deleteUser(user_id);
            if(deleted == true)
            {
                return StatusCode((int)HttpStatusCode.NoContent, "Deleted user successfully");
            }
            return StatusCode(404);
        }

        [HttpPut]
        [Route("/api/v1/users/{user_id}")]
        public virtual IActionResult UsersUserIdPut([FromRoute]int user_id, [FromBody]CreateUser user)
        { 
            var toUpdate = _UserService.getUserById(user_id);

            CreatedUser result;

            if (toUpdate == null || user == null)
            {
                return StatusCode(404);
            }


            if (user_id == toUpdate.Id)
            {
                if(ModelState.IsValid)
                {
                    result = _UserService.editUser(user_id, user);
                }
                else
                {
                    return StatusCode(412, "User object invalid");
                }
            }
            else
            {
                return NotFound("UserId is incorrect");
            }

            if(result != null)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(404);
            }
        }

    #endregion

    #region Users -> Books

        /// <summary>
        /// Gets books
        /// </summary>
        /// <remarks>Returns all books for a person.</remarks>
        /// <param name="user_id">The users&#39;s user_id.</param>
        /// <response code="200">Books</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Unknown error occurred.</response>
        [HttpGet]
        [Route("/api/v1/users/{user_id}/books")]
        public virtual IActionResult UsersUserIdBooksGet([FromRoute]int user_id)
        { 
            IEnumerable<BookDTO> loans = null;
            
            //check if user_id exists
            var userExists = _UserService.getUserById(user_id);

            if(userExists != null)
            {
                loans = _BookService.getBooksOnLoanByUser(user_id);
            }
            else
            {
                return StatusCode(404, "UserId not found");
            }

            if(loans != null)
            {
                return Ok(loans);
            }
            else
            {
                return StatusCode(404);
            }
        }

        /// <summary>
        /// Posts a book to a user
        /// </summary>
        /// <remarks>Posts a book to a user.</remarks>
        /// <param name="book_id">The books book_id.</param>
        /// <param name="user_id">The person&#39;s user_id.</param>
        /// <response code="200">A book</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Unknown error occurred.</response>
        [HttpPost]
        [Route("/api/v1/users/{user_id}/books/{book_id}")]
        public virtual IActionResult UsersUserIdBooksBookIdPost([FromRoute]int book_id, [FromRoute]int user_id)
        { 
            var user = _UserService.getUserById(user_id);
            var book = _BookService.getBookById(book_id);

            if(user == null && book == null)
            {
                return StatusCode(404, "UserID and bookId invalid");
            }
            else if(user != null && book == null)
            {
                return StatusCode(404, "BookId not found");
            }
            else if(user == null && book != null)
            {
                return StatusCode(404, "UserId not found");
            }
            else
            {
                var happend = _BookService.addBookToUser(book_id, user_id);
                if(happend) 
                {
                    return StatusCode((int)HttpStatusCode.Created, "Book: " + book.Title + " added to: " + user.Name);
                }
                else
                {
                    return StatusCode(404,  "Book already loaned to user");
                }
            }
        }

        /// <summary>
        /// Deletes a book for a user
        /// </summary>
        /// <remarks>Delete a single book identified via its id for a user</remarks>
        /// <param name="book_id">The book&#39;s book_id</param>
        /// <param name="user_id">The person&#39;s user_id.</param>
        /// <response code="204">book successfully deleted.</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Unknown error occurred.</response>
        [HttpDelete]
        [Route("/api/v1/users/{user_id}/books/{book_id}")]
        public virtual IActionResult UsersUserIdBooksBookIdDelete([FromRoute]int book_id, [FromRoute]int user_id)
        { 
            var user = _UserService.getUserById(user_id);
            var book = _BookService.getBookById(book_id);

            if(user == null && book == null)
            {
                return StatusCode(404, "UserID and bookId invalid");
            }
            else if(user != null && book == null)
            {
                return StatusCode(404, "BookId not found");
            }
            else if(user == null && book != null)
            {
                return StatusCode(404, "UserId not found");
            }
            else
            {
                var happend = _BookService.deleteBookFromUser(book_id, user_id);
                if(happend) 
                {
                    return StatusCode((int)HttpStatusCode.Created, "Book: " + book.Title + " returned by: " + user.Name);
                }
                else
                {
                    return StatusCode(404,  "Book not in loan by user");
                }
            }
        }

        /// <summary>
        /// Changes a book for a user
        /// </summary>
        /// <remarks>Changes a single book for its id for a user.</remarks>
        /// <param name="book_id">The book&#39;s book_id</param>
        /// <param name="user_id">The person&#39;s user_id.</param>
        /// <param name="book">The loan to update.</param>
        /// <response code="200">A loan</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Unknown error occurred.</response>
        [HttpPut]
        [Route("/api/v1/users/{user_id}/books/{book_id}")]
        public virtual IActionResult UsersUserIdBooksBookIdPut([FromRoute]int book_id, [FromRoute]int user_id, [FromBody]UserLoan loan)
        { 
            var toUpdate = _BookService.getBookUser(book_id, user_id);
    
            bool result = false;

            if (toUpdate == null || loan == null)
            {
                return StatusCode(404);
            }

            if(ModelState.IsValid)
            {
                result = _BookService.editBookUser(book_id, user_id, loan);
            }
            else
            {
                return StatusCode(404, "Update loan object invalid");
            }
            
            if(result)
            {
                var updatedLoan = _BookService.getBookUser(book_id, user_id);
                return Ok(updatedLoan);
            }
            else
            {
                return StatusCode(404);
            }
        }

    #endregion

    #region Users -> Reviews
        
        /// <summary>
        /// Gets book reviews for a user
        /// </summary>
        /// <remarks>Returns a list of reviews for a user.</remarks>
        /// <param name="user_id">The users user_id.</param>
        /// <response code="200">A list of reviews for the book.</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Unknown error occurred.</response>
        [HttpGet]
        [Route("/api/v1/users/{user_id}/reviews")]
        public virtual IActionResult UsersUserIdReviewsGet([FromRoute]int user_id)
        { 
            var user = _UserService.getUserById(user_id);
            
            if(user == null)
            {
                return StatusCode(404, "UserId not found");
            }
            return Ok(_ReviewService.getReviewsById(user_id));
        }

        /// <summary>
        /// Gets a users review for a book
        /// </summary>
        /// <remarks>Returns a single users review for a specific book.</remarks>
        /// <param name="user_id">The users user_id.</param>
        /// <param name="book_id">The books book_id</param>
        /// <response code="200">A review</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Unknown error occurred.</response>
        [HttpGet]
        [Route("/api/v1/users/{user_id}/reviews/{book_id}")]
        public virtual IActionResult UsersUserIdReviewsBookIdGet([FromRoute]int user_id, [FromRoute]int book_id)
        { 
            var user = _UserService.getUserById(user_id);
            var book = _BookService.getBookById(book_id);

            if(user == null && book == null)
            {
                return StatusCode(404, "UserID and bookId invalid");
            }
            else if(user != null && book == null)
            {
                return StatusCode(404, "BookId not found");
            }
            else if(user == null && book != null)
            {
                return StatusCode(404, "UserId not found");
            }
            else
            {
                var retReview = _ReviewService.getReviewById(book_id, user_id);

                if(retReview != null)
                {
                    return Ok(retReview);
                }
                else
                {
                    return StatusCode(404);
                }
            }
        }


        /// <summary>
        /// Posts a review
        /// </summary>
        /// <remarks>Posts a single review.</remarks>
        /// <param name="user_id">The users&#39;s user_id</param>
        /// <param name="book_id">The books book_id</param>
        /// <response code="204">review successfully created.</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Unknown error occurred.</response>
        [HttpPost]
        [Route("/api/v1/users/{user_id}/reviews/{book_id}")]
        public virtual IActionResult UsersUserIdReviewsBookIdPost([FromRoute]int user_id, [FromRoute]int book_id, [FromBody]ReviewViewModel review)
        { 
            var user = _UserService.getUserById(user_id);
            var book = _BookService.getBookById(book_id);

            if(user == null && book == null)
            {
                return StatusCode(404, "UserID and bookId invalid");
            }
            else if(user != null && book == null)
            {
                return StatusCode(404, "BookId not found");
            }
            else if(user == null && book != null)
            {
                return StatusCode(404, "UserId not found");
            }
            else
            {
                var addedReview = _ReviewService.addBookReviewFromUser(book_id, user_id, review);

                if (addedReview != null)
                {
                    return Ok(addedReview);
                }
                else
                {
                    return StatusCode(500, "Unknown error occurred.");
                }
            }
        }


        /// <summary>
        /// Deletes a review
        /// </summary>
        /// <remarks>Delete a single review identified via its user_id and book_id.</remarks>
        /// <param name="user_id">The users&#39;s user_id</param>
        /// <param name="book_id">The books book_id</param>
        /// <response code="204">review successfully deleted.</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Unknown error occurred.</response>
        [HttpDelete]
        [Route("/api/v1/users/{user_id}/reviews/{book_id}")]
        public virtual IActionResult UsersUserIdReviewsBookIdDelete([FromRoute]int user_id, [FromRoute]int book_id)
        { 
            var user = _UserService.getUserById(user_id);
            var book = _BookService.getBookById(book_id);

            if(user == null && book == null)
            {
                return StatusCode(404, "UserID and bookId invalid");
            }
            else if(user != null && book == null)
            {
                return StatusCode(404, "BookId not found");
            }
            else if(user == null && book != null)
            {
                return StatusCode(404, "UserId not found");
            }
            else
            {
                _ReviewService.deleteReviewFromUser(book_id, user_id);
                return StatusCode(204, "Review successfully deleted");
            }
        }

        /// <summary>
        /// Changes a review
        /// </summary>
        /// <remarks>Changes a single review for a specific book.</remarks>
        /// <param name="user_id">The users&#39;s user_id</param>
        /// <param name="book_id">The books book_id</param>
        /// <param name="review">The review to update.</param>
        /// <response code="200">A review</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Unknown error occurred.</response>
        [HttpPut]
        [Route("/api/v1/users/{user_id}/reviews/{book_id}")]
        public virtual IActionResult UsersUserIdReviewsBookIdPut([FromRoute]int user_id, [FromRoute]int book_id, [FromBody]ReviewViewModel review)
        { 
            var user = _UserService.getUserById(user_id);
            var book = _BookService.getBookById(book_id);

            if(!ModelState.IsValid)
            {
                return StatusCode(404, "BookId not found");
            }

            if(user == null && book == null)
            {
                return StatusCode(404, "UserID and bookId invalid");
            }
            else if(user != null && book == null)
            {
                return StatusCode(404, "BookId not found");
            }
            else if(user == null && book != null)
            {
                return StatusCode(404, "UserId not found");
            }
            else
            {
                var updatedReview = _ReviewService.editBookReviewFromUser(book_id, user_id, review);

                if (updatedReview != null)
                {
                    return StatusCode(204, updatedReview);
                }
                else
                {
                    return StatusCode(500, "Unknown error occurred.");
                }
            }
        }

    #endregion

    #region Users -> Recommendation

        // <summary>
        /// Gets a list of book recommendations for a user
        /// </summary>
        /// <remarks>Returns a list of book recommendations for a user</remarks>
        /// <param name="user_id">The users user_id.</param>
        /// <response code="200">A list of book recommendations for a user</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Unknown error occurred.</response>
        [HttpGet]
        [Route("/api/v1/users/{user_id}/recommendation")]
        public virtual IActionResult UsersUserIdRecommendationGet([FromRoute]int user_id)
        { 
            var validUser = _UserService.getUserById(user_id);
 
            if(validUser == null)
           {
                return StatusCode(404, "User not found");
            }

            var recList = _RecommendationService.getRecommendations(user_id);

            if(recList != null)
            {
                return Ok(recList);
            }
            else
            {
                return StatusCode(500, "Unknown error occurred.");
            }
        }

    #endregion

    #region Books -> Reviews
        

        /// <summary>
        /// Gets all book Reviews
        /// </summary>
        /// <remarks> Gets all book reviews from all users
        /// <response code="200"> Reviews found </response>
         /// <response code="404"> Reviews not found </response>
        [HttpGet]
        [Route("/api/v1/books/reviews")]
        public virtual IActionResult ReviewsGet()
        {
            var user = _ReviewService.getBookReview();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        /// <summary>
        /// Gets a book review
        /// </summary>
        /// <remarks> Gets a single book review from all users
        /// <response code="200"> Reviews found </response>
        /// <response code="404"> Reviews not found </response>
        [HttpGet]
        [Route("/api/v1/books/{book_Id}/reviews")]
        public virtual IActionResult ReviewGet(int book_Id)
        {
            var bookReview = _ReviewService.getBookReviewById(book_Id);
            if (bookReview == null)
            {
                return NotFound();
            }
            return Ok(bookReview);
        }
        
        
        /// <summary>
        /// Gets Users Review of single book
        /// </summary>
        /// <remarks>Gets a single Review from user.</remarks>
        /// <param name="user_id">The users&#39;s user_id</param>
        /// <param name="book_id">The books book_id</param>
        /// <param name="review">The review to update.</param>
        /// <response code="200">A review</response>
        /// <response code="404">Not found.</response>
        [HttpGet]
        [Route("/api/v1/books/{book_id}/reviews/{user_id}")]
        public virtual IActionResult UserBookReview([FromRoute] int? book_id, [FromRoute]int? user_id)
        {
            if (!user_id.HasValue || !book_id.HasValue)
            {
                return NotFound();
            }
            var Review =_ReviewService.getBookReviewFromUser(book_id.Value, user_id.Value);
            if (Review == null)
            {
                return NotFound();
            }
            return Ok(Review);
        }

        /// <summary>
        /// Changes a review
        /// </summary>
        /// <remarks>Changes a single review for a specific book.</remarks>
        /// <param name="user_id">The users&#39;s user_id</param>
        /// <param name="book_id">The books book_id</param>
        /// <param name="review">The review to update.</param>
        /// <response code="200">A review</response>
        /// <response code="404">Not found.</response>
        [HttpPut]
        [Route("/api/v1/books/{book_id}/reviews/{user_id}")]
        public virtual IActionResult BooksUserIdReviewsBookIdPut([FromRoute]int? book_id, [FromRoute]int? user_id, [FromBody]ReviewViewModel review)
        { 
            if (user_id == null || book_id == null)
            {
                return NotFound();
            }
            if (review.UserReview == null)
            {
                return BadRequest();
            }
            var editReview = _ReviewService.editBookReviewFromUser(book_id.Value, user_id.Value, review);
            if (editReview == null){
                return NotFound();
            }
            return StatusCode(201);

        }

        /// <summary>
        /// Deletes a review
        /// </summary>
        /// <remarks>Delete a single review identified via its user_id and book_id.</remarks>
        /// <param name="user_id">The users&#39;s user_id</param>
        /// <param name="book_id">The books book_id</param>
        /// <response code="204">review successfully deleted.</response>
        /// <response code="404">Not found.</response>
        /// <response code="500">Unknown error occurred.</response>
        [HttpDelete]
        [Route("/api/v1/books/{book_id}/reviews/{user_id}")]
        public virtual IActionResult BooksUserIdReviewsBookIdDelete([FromRoute]int? book_id, [FromRoute]int? user_id)
        { 
            if (user_id == null || book_id == null)
            {
                return NotFound();
            }
            var RemovedReview = _ReviewService.deleteReviewFromUser(user_id.Value, book_id.Value);
            if (RemovedReview == null)
            {
                return NotFound();
            }
            return StatusCode(204);

        }

    #endregion

    }
}