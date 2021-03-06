using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Api;
using FluentAssertions;
using BookApi.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookApi.Models.DTOModels;
using BookApi.Models.Entities;
using BookApi.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BookApi.Tests
{
    [TestClass]
    public class ControllerIntegrationTests
    {
        #region Setup
        public HttpClient client;
        private TestServer _server;



        /// <summary>
        /// Initialize tests for integrations 
        /// Where it tests end-to-end tests
        /// We are using testDatabase so we startup
        /// the program with testStartup instead of normal
        /// startup.
        /// </summary>        
        [TestInitialize]
        public void ControllerSetup()
        {
            var builder = new WebHostBuilder().UseStartup<TestStartup>();
            _server = new TestServer(builder);
            client = _server.CreateClient(); 

        }
		
        #endregion

		#region Books

        #region HappyPathsBooks
        //GET on /books DONE



        /// <summary>
        /// Get All boos tests 
        /// path: /api/v1/books
        /// method: GET
        /// returns all boosk in database;
        /// </summary>
        [TestMethod]
        public void getBooks()
        {
            List<BookDTO> books = null;
            var response = client.GetAsync("/api/v1/books")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                books = JsonConvert.DeserializeObject<List<BookDTO>>(stringur.Result);

            });
            response.Wait();
            Assert.IsNotNull(books);
            Assert.AreEqual(books.Count, 1000);
            Assert.AreEqual(books[0].Title, "Compatible clear-thinking neural-net");
        }


        /// <summary>
        /// Get test of books that has had book loaned more than 30 days
        /// path: /api/v1/books?LoanDate=2017-10-10&LoanDuration=30
        /// method: GET
        /// </summary>
        [TestMethod]
        public void getBooksByLoanDateAndLoanDuration()
        {
            List<BookDTO> books = null;
            var response = client.GetAsync("/api/v1/books?LoanDate=2017-10-10&LoanDuration=30")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                books = JsonConvert.DeserializeObject<List<BookDTO>>(stringur.Result);

            });
            response.Wait();
            Assert.IsNotNull(books);
            Assert.AreEqual(books.Count, 36);
            Assert.AreEqual(books[0].Title, "Integrated impactful internet solution");
        }
       
       
         /// <summary>
        /// Posts a single book tests 
        /// path: /api/v1/books
        /// method: POST
        /// returns book that was created
        /// </summary>
        [TestMethod]
        public void createBook()
        {
            //Arrange
            BookDTO createdBook = null;
            NewBook book = new NewBook {
                Title = "New test book",
                Author = "Author",
                DatePublished = "2017-01-01",
                ISBN = "123456789"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");

            //Act
            var response = client.PostAsync("/api/v1/books", stringContent)
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                createdBook = JsonConvert.DeserializeObject<BookDTO>(stringur.Result);

            });

            response.Wait();


            //Cleanup
            //delete the book that we just created 
            var bookId = createdBook.Id;
            var deleteResponse = client.DeleteAsync($"/api/v1/books/{bookId}");
            deleteResponse.Wait();

            //Assert
            Assert.IsTrue(response.IsCompletedSuccessfully);
            Assert.IsNotNull(createdBook);
            Assert.AreEqual(createdBook.Title, book.Title);
            Assert.AreEqual(createdBook.Author, book.Author);
            Assert.AreEqual(createdBook.DatePublished, book.DatePublished);
            Assert.AreEqual(createdBook.ISBN, book.ISBN);
        }



        /// <summary>
        /// Gets a single book by Id
        /// path: /api/v1/books/"
        /// method: GET
        /// </summary>
        //GET on /books/{book_id} DONE
        [TestMethod]
        public void getBookById()
        {
            //Act
            BookAndLoansDTO book = null;
            var getResponse = client.GetAsync("/api/v1/books/" + 413)
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                book = JsonConvert.DeserializeObject<BookAndLoansDTO>(stringur.Result);

            });

            getResponse.Wait();
            
            Assert.IsTrue(getResponse.IsCompletedSuccessfully);
            Assert.IsNotNull(book);
            Assert.AreEqual(book.Title, "Fundamental fresh-thinking data-warehouse");
            Assert.AreEqual(book.Author, "Derk Hawk");
            Assert.AreEqual(book.DatePublished, "1988-12-22");
            Assert.AreEqual(book.ISBN, "617486634-3");
            Assert.IsNotNull(book.LoanHistory);
        }

        /// <summary>
        /// Deletes a single book by Id
        /// path: /api/v1/books/"
        /// method: DELETE
        /// </summary>
        //DELETE on /books/{book_id} DONE
        [TestMethod]
        public void deleteBookById()
        {
            //Create new book and add it to db
            //Get newly created book from db
            //Delete newly created book
            //Try to get book again and assert

            //Arrange
            //Create new book and add it to db
            //Get newly created book from db
            BookDTO createdBook = null;
            NewBook newBook = new NewBook {
                Title = "New test book",
                Author = "Author",
                DatePublished = "2017-01-01",
                ISBN = "123456789"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(newBook), Encoding.UTF8, "application/json");

            var response = client.PostAsync("/api/v1/books", stringContent)
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                createdBook = JsonConvert.DeserializeObject<BookDTO>(stringur.Result);
            });

            response.Wait();

            BookAndLoansDTO bookToDelete = null;
            var getResponse = client.GetAsync("/api/v1/books/" + createdBook.Id)
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                bookToDelete = JsonConvert.DeserializeObject<BookAndLoansDTO>(stringur.Result);
            });

            getResponse.Wait();

            //Act
            //delete the book that we just created 
            var deleteResponse = client.DeleteAsync("/api/v1/books/" + createdBook.Id);
            deleteResponse.Wait();

            Assert.IsTrue(deleteResponse.IsCompletedSuccessfully);
            Assert.IsNotNull(bookToDelete);
            Assert.AreEqual(bookToDelete.Title, createdBook.Title);
            Assert.AreEqual(bookToDelete.Author, createdBook.Author);
            Assert.AreEqual(bookToDelete.DatePublished, createdBook.DatePublished);
            Assert.AreEqual(bookToDelete.ISBN, createdBook.ISBN);

            //Try to get the book again
            BookAndLoansDTO bookShouldBeNull = null;
            var getAgainResponse = client.GetAsync("/api/v1/books/" + createdBook.Id)
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                bookShouldBeNull = JsonConvert.DeserializeObject<BookAndLoansDTO>(stringur.Result);
            });

            getResponse.Wait();

            Assert.IsNull(bookShouldBeNull);
        }


        /// <summary>
        /// Changes a book by Id
        /// path: /api/v1/books/"
        /// method: PUT
        /// </summary>
        //PUT on /books/{book_id} DONE
        [TestMethod]
        public void changeBookById()
        {
            //Create new book and add it to db
            //Get newly created book from db
            //Edit newly created book
            //Get book again and assert changes
            //Delete book

            //Arrange
            //Create new book and add it to db
            //Get newly created book from db
            BookDTO createdBook = null;
            NewBook newBook = new NewBook {
                Title = "New test book",
                Author = "Author",
                DatePublished = "2017-01-01",
                ISBN = "123456789"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(newBook), Encoding.UTF8, "application/json");

            //Create book
            var response = client.PostAsync("/api/v1/books", stringContent)
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                createdBook = JsonConvert.DeserializeObject<BookDTO>(stringur.Result);
            });

            response.Wait();

            BookAndLoansDTO bookToEdit = null;
            var getResponse = client.GetAsync("/api/v1/books/" + createdBook.Id)
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                bookToEdit = JsonConvert.DeserializeObject<BookAndLoansDTO>(stringur.Result);
            });
            getResponse.Wait();

            //Act
            //edit the book that we just created 
            NewBook editedBook = new NewBook {
                Title = "Edited test book",
                Author = "Edited Author",
                DatePublished = "2017-12-12",
                ISBN = "987654321"
            };

            var stringEditedContent = new StringContent(JsonConvert.SerializeObject(editedBook), Encoding.UTF8, "application/json");
            
            var editResponse = client.PutAsync("/api/v1/books/" + createdBook.Id, stringEditedContent);
            editResponse.Wait();

            //Get the book again and assert changes
            BookAndLoansDTO bookShouldBeEdited = null;
            var getAgainResponse = client.GetAsync("/api/v1/books/" + createdBook.Id)
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                bookShouldBeEdited = JsonConvert.DeserializeObject<BookAndLoansDTO>(stringur.Result);
            });

            getAgainResponse.Wait();

            var deleteResponse = client.DeleteAsync("/api/v1/books/" + createdBook.Id);
            deleteResponse.Wait();

            Assert.IsNotNull(bookShouldBeEdited);
            Assert.IsTrue(editResponse.IsCompletedSuccessfully);
            Assert.AreEqual(bookShouldBeEdited.Id, createdBook.Id);
            Assert.AreEqual(bookShouldBeEdited.Title, editedBook.Title);
            Assert.AreEqual(bookShouldBeEdited.Author, editedBook.Author);
            Assert.AreEqual(bookShouldBeEdited.DatePublished, editedBook.DatePublished);
            Assert.AreEqual(bookShouldBeEdited.ISBN, editedBook.ISBN);
        }

        #endregion

        #region FailingPathsBooks

        //Post a book with all fields empty

        //Put a book with an empty field

        #endregion

		#endregion

        #region Users

        #region HappyPathsUsers
        
        /// <summary>
        /// Get all users from database
        /// path: /api/v1/users
        /// method: GET
        /// </summary>
        [TestMethod]
        public void getUsers()
        {
            List<CreatedUser> users = null;
            var response = client.GetAsync("/api/v1/users")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                users = JsonConvert.DeserializeObject<List<CreatedUser>>(stringur.Result);
                Assert.AreEqual(res.ReasonPhrase, "OK");
            });
            response.Wait();
            Assert.IsTrue(response.IsCompletedSuccessfully);
            Assert.IsNotNull(users);
            Assert.AreEqual(users.Count, 100);
            Assert.AreEqual(users[0].Name, "Nelli Riglesford");
        }


        /// <summary>
        /// Gets a user by id from database
        /// path: /api/v1/users/{user_id}
        /// method: GET
        /// </summary>
        [TestMethod]
        public void getUsersByLoanDateAndLoanDuration()
        {
            List<CreatedUser> users = null;
            var response = client.GetAsync("/api/v1/users?LoanDate=2017-10-10&LoanDuration=30")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                users = JsonConvert.DeserializeObject<List<CreatedUser>>(stringur.Result);
                Assert.AreEqual(res.ReasonPhrase, "OK");
            });
            response.Wait();
            Assert.IsTrue(response.IsCompletedSuccessfully);
            Assert.IsNotNull(users);
            Assert.AreEqual(users.Count, 36);
            Assert.AreEqual(users[0].Name, "Pierette Klawi");
        }
        [TestMethod]
        public void getUserById()
        {
            int user_id = 1;
            CreatedUser user = null;
            var response = client.GetAsync($"/api/v1/users/{user_id}")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                user = JsonConvert.DeserializeObject<CreatedUser>(stringur.Result);
                Assert.AreEqual(res.ReasonPhrase, "OK");
            });
            response.Wait();
            Assert.IsTrue(response.IsCompletedSuccessfully);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.Name, "Nelli Riglesford");
            Assert.AreEqual(user.Address, "16 Fallview Circle");
            Assert.AreEqual(user.Email, "nriglesford0@phoca.cz");
            Assert.AreEqual(user.PhoneNumber, null);
        }

        [TestMethod]
        public void postAUser()
        {
            CreatedUser retUser = null;
            CreateUser user = new CreateUser{
                Name = "Tester Testerson",
                Address = "Testergötu 1",
                Email = "test@test.is",
                PhoneNumber = "5812345"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");


            //Act
            var response = client.PostAsync("/api/v1/users", stringContent)
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                retUser = JsonConvert.DeserializeObject<CreatedUser>(stringur.Result);
            });

            response.Wait();
            var deleteResponse = client.DeleteAsync($"/api/v1/users/{retUser.Id}");
            Assert.IsTrue(response.IsCompletedSuccessfully);
            Assert.IsNotNull(retUser);
            Assert.AreEqual(retUser.Name, user.Name);
            Assert.AreEqual(retUser.Address, user.Address);
            Assert.AreEqual(retUser.Email, user.Email);
            Assert.AreEqual(retUser.PhoneNumber, user.PhoneNumber);
        }


        /// <summary>
        /// Edit a user 
        /// path: /api/v1/users/{user_id}
        /// method: PUT
        /// </summary>
        [TestMethod]
        public void putAUser()
        {
            CreatedUser retUser = null;
            CreatedUser originalUser = new CreatedUser{
                Id = 1,
                Name = "Nelli Riglesford",
                Address = "16 Fallview Circle",
                Email = "nriglesford0@phoca.cz",
                PhoneNumber = null
            };
            CreateUser changeUser = new CreateUser{
                Name = "Tester Testerson",
                Address = "Testergötu 1",
                Email = "test@test.is",
                PhoneNumber = "5812345"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(changeUser), Encoding.UTF8, "application/json");


            //Act
            var response = client.PutAsync($"/api/v1/users/{originalUser.Id}", stringContent)
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                retUser = JsonConvert.DeserializeObject<CreatedUser>(stringur.Result);
            });

            response.Wait();
            Assert.IsTrue(response.IsCompletedSuccessfully);
            Assert.IsNotNull(retUser);
            Assert.AreEqual(retUser.Name, changeUser.Name);
            Assert.AreEqual(retUser.Address, changeUser.Address);
            Assert.AreEqual(retUser.Email, changeUser.Email);
            Assert.AreEqual(retUser.PhoneNumber, changeUser.PhoneNumber);

            //Change back to original
            CreatedUser retUserBack = null;
            var stringContentBack = new StringContent(JsonConvert.SerializeObject(originalUser), Encoding.UTF8, "application/json");
            var responseBack = client.PutAsync($"/api/v1/users/{originalUser.Id}", stringContentBack)
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                retUserBack = JsonConvert.DeserializeObject<CreatedUser>(stringur.Result);
            });
            responseBack.Wait();
            Assert.IsTrue(responseBack.IsCompletedSuccessfully);
            Assert.IsNotNull(retUserBack);
            Assert.AreEqual(retUserBack.Name, originalUser.Name);
            Assert.AreEqual(retUserBack.Address, originalUser.Address);
            Assert.AreEqual(retUserBack.Email, originalUser.Email);
            Assert.AreEqual(retUserBack.PhoneNumber, originalUser.PhoneNumber);
        }


        /// <summary>
        /// Deletes a user by Id
        /// path: /api/v1/users/{user_id}
        /// method: DELETE
        /// </summary>
        [TestMethod]
        public void deleteAUser()
        {
            CreatedUser retUser = null;
            CreateUser user = new CreateUser{
                Name = "Tester Testerson",
                Address = "Testergötu 1",
                Email = "test@test.is",
                PhoneNumber = "5812345"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");


            //Add the user first
            var response = client.PostAsync("/api/v1/users", stringContent)
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                retUser = JsonConvert.DeserializeObject<CreatedUser>(stringur.Result);
            });

            response.Wait();

            string message = "";
            var deleteResponse = client.DeleteAsync($"/api/v1/users/{retUser.Id}")            
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                message = stringur.Result;
                Assert.AreEqual(message, "Deleted user successfully");
            });
            deleteResponse.Wait();

            Assert.IsTrue(deleteResponse.IsCompletedSuccessfully);
            Assert.IsNotNull(retUser);
            Assert.AreEqual(retUser.Name, user.Name);
            Assert.AreEqual(retUser.Address, user.Address);
            Assert.AreEqual(retUser.Email, user.Email);
            Assert.AreEqual(retUser.PhoneNumber, user.PhoneNumber);

            CreatedUser userShouldBeNull = null;
            var responseGet = client.GetAsync($"/api/v1/users/{retUser.Id}")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                userShouldBeNull = JsonConvert.DeserializeObject<CreatedUser>(stringur.Result);
                Assert.AreEqual(res.ReasonPhrase, "No Content");
            });
            responseGet.Wait();
            
            Assert.IsTrue(responseGet.IsCompletedSuccessfully);
            Assert.IsNull(userShouldBeNull);
        }

        #endregion

        #region FailingPathsUsers
        

        /// <summary>
        /// Fail test tries to add user with no Name
        /// path: /api/v1/users
        /// method: POST
        /// </summary>
        [TestMethod]
        public void addUserWithNoName()
        {
            CreateUser user = new CreateUser{
                Name = "",
                Address = "Testergötu 1",
                Email = "test@test.is",
                PhoneNumber = "5812345"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            string message = "";
            //Act
            var response = client.PostAsync("/api/v1/users", stringContent)
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                message = stringur.Result;
            });

            response.Wait();
            Assert.IsTrue(response.IsCompletedSuccessfully);
            Assert.AreEqual(message, "User must have a name");
        }

        #endregion

        #endregion

		#region Users -> Books    

        #region HappyPathsUsersBooks
        
        
        
        /// <summary>
        /// Get a Books that are loaned by userId
        /// path: /api/v1/users/{user_id}/books/
        /// method: GET
        /// </summary>
        [TestMethod]
        public void GetBooksOnLoanByUserId()
        {
            List<BookDTO> booksOnLoan = null;
            int user_id = 90;
            var response = client.GetAsync($"/api/v1/users/{user_id}/books/")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                booksOnLoan = JsonConvert.DeserializeObject<List<BookDTO>>(stringur.Result);

            });
            response.Wait();

            Assert.IsNotNull(booksOnLoan);
            Assert.AreEqual(booksOnLoan.Count, 3);
        }





        /// <summary>
        /// Add book that is loaned by User
        /// path: /api/v1/users/{user_id}/books/{book_id}
        /// method: POST
        /// </summary>
        [TestMethod]
        public async Task AddBookOnLoanByUser()
        {
            Random r = new Random();
            int userInt = r.Next(1, 100); 
            int bookInt = r.Next(1,1000);

            int user_id = userInt;
            int book_id = bookInt;

            String today = DateTime.Now.ToString("yyyy-MM-dd");

            UserLoan newLoan = new UserLoan {
                LoanDate = today,
                ReturnedDate = "0"
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(newLoan), Encoding.UTF8, "application/json");

            var postResponse = await client.PostAsync($"/api/v1/users/{user_id}/books/{book_id}", stringContent);

            List<BookDTO> booksOnLoan = null;
            var getResponse = client.GetAsync($"/api/v1/users/{user_id}/books/")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                booksOnLoan = JsonConvert.DeserializeObject<List<BookDTO>>(stringur.Result);
            });
            getResponse.Wait();

            //Get book by Id
            BookAndLoansDTO book = null;
            var getBookResponse = client.GetAsync("/api/v1/books/" + book_id)
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                book = JsonConvert.DeserializeObject<BookAndLoansDTO>(stringur.Result);
            });
            getBookResponse.Wait();


            Assert.IsNotNull(booksOnLoan);
            Assert.AreEqual(booksOnLoan[booksOnLoan.Count - 1].Id, book_id);
            Assert.AreEqual(booksOnLoan[booksOnLoan.Count - 1].Title, book.Title);
            Assert.AreEqual(booksOnLoan[booksOnLoan.Count - 1].Author, book.Author);
            Assert.AreEqual(booksOnLoan[booksOnLoan.Count - 1].DatePublished, book.DatePublished);
            Assert.AreEqual(booksOnLoan[booksOnLoan.Count - 1].ISBN, book.ISBN);

            //Delete loan
            var deleteResponse = await client.DeleteAsync($"/api/v1/users/{user_id}/books/{book_id}");
        }

        //DELETE on /users/{user_id}/books/{book_id} DONE


        /// <summary>
        /// Removes a Book that is loaned by user by userId
        /// path: /api/v1/users/{user_id}/books/
        /// method: POST
        /// </summary>
        [TestMethod]
        public async Task DeleteBookOnLoanByUser()
        {
            //Add new loan
            //Delete loan
            //Check if loan still exists

            Random r = new Random();
            int userInt = r.Next(1, 100); 
            int bookInt = r.Next(1,1000);

            int user_id = userInt;
            int book_id = bookInt;

            String today = DateTime.Now.ToString("yyyy-MM-dd");

            UserLoan newLoan = new UserLoan {
                LoanDate = today,
                ReturnedDate = "0"
            };

            //Add new loan
            var stringContent = new StringContent(JsonConvert.SerializeObject(newLoan), Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync($"/api/v1/users/{user_id}/books/{book_id}", stringContent);

            //Get list of books on loan by user
            List<BookDTO> booksOnLoan = null;
            var getResponse = client.GetAsync($"/api/v1/users/{user_id}/books/")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                booksOnLoan = JsonConvert.DeserializeObject<List<BookDTO>>(stringur.Result);
            });
            getResponse.Wait();

            int nrOfBooksOnLoan = booksOnLoan.Count;

            //Delete loan
            var deleteResponse = await client.DeleteAsync($"/api/v1/users/{user_id}/books/{book_id}");

            //Get again list of books on loan by user
            List<BookDTO> booksAfterDeleteOnLoan = null;
            var getAfterDeleteResponse = client.GetAsync($"/api/v1/users/{user_id}/books/")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                booksAfterDeleteOnLoan = JsonConvert.DeserializeObject<List<BookDTO>>(stringur.Result);
            });
            getAfterDeleteResponse.Wait();

            Assert.AreNotEqual(booksAfterDeleteOnLoan.Count, nrOfBooksOnLoan);
            Assert.AreEqual(booksAfterDeleteOnLoan.Count, nrOfBooksOnLoan - 1);
        }

        /// <summary>
        /// edits a Book that is loaned by user by userId
        /// path: /api/v1/users/{user_id}/books/{book_id}
        /// method: PUT
        /// </summary>
        [TestMethod]
        public async Task EditBookOnLoanByUser()
        {
            //Add new loan
            //Change loan date so its returned
            //Verify that loan does not show
            //Cange back returnedDate
            //Verify that loan does show
            //Delete loan
            
            Random r = new Random();
            int userInt = r.Next(1, 100); 
            int bookInt = r.Next(1,1000);

            int user_id = userInt;
            int book_id = bookInt;

            String today = DateTime.Now.ToString("yyyy-MM-dd");

            UserLoan newLoan = new UserLoan {
                LoanDate = today,
                ReturnedDate = "0"
            };
            UserLoan editedLoan = new UserLoan {
                LoanDate = "2015-12-12",
                ReturnedDate = "2017-12-12"
            };
            UserLoan editedBackLoan = new UserLoan {
                LoanDate = today,
                ReturnedDate = "0"
            };

            //Add new loan
            var stringContent = new StringContent(JsonConvert.SerializeObject(newLoan), Encoding.UTF8, "application/json");
            var postResponse = client.PostAsync($"/api/v1/users/{user_id}/books/{book_id}", stringContent);
            postResponse.Wait();

            //Get list of books on loan by user
            List<BookDTO> booksOnLoan = null;
            var getResponse = client.GetAsync($"/api/v1/users/{user_id}/books/")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                booksOnLoan = JsonConvert.DeserializeObject<List<BookDTO>>(stringur.Result);
            });
            getResponse.Wait();

            int nrOfBooksOnLoan = booksOnLoan.Count;

            var editedLoanContent = new StringContent(JsonConvert.SerializeObject(editedLoan), Encoding.UTF8, "application/json");
            var editResponse = client.PutAsync($"/api/v1/users/{user_id}/books/{book_id}", editedLoanContent);
            editResponse.Wait();

            //Get again list of books on loan by user
            List<BookDTO> booksAfterEditedLoan = null;
            var getAfterEditedResponse = client.GetAsync($"/api/v1/users/{user_id}/books/")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                booksAfterEditedLoan = JsonConvert.DeserializeObject<List<BookDTO>>(stringur.Result);
            });
            getAfterEditedResponse.Wait();

            int nrOfBooksOnLoanAfterEdit = booksAfterEditedLoan.Count;

            Assert.AreNotEqual(booksAfterEditedLoan.Count, nrOfBooksOnLoan);
            Assert.AreEqual(booksAfterEditedLoan.Count, nrOfBooksOnLoan - 1);

            //Change loan dates back to original
            var editedBackLoanContent = new StringContent(JsonConvert.SerializeObject(editedBackLoan), Encoding.UTF8, "application/json");
            var editBackResponse = await client.PutAsync($"/api/v1/users/{user_id}/books/{book_id}", editedBackLoanContent);

            List<BookDTO> booksAfterEditedBackLoan = null;
            var getAfterEditedBackResponse = client.GetAsync($"/api/v1/users/{user_id}/books/")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                booksAfterEditedBackLoan = JsonConvert.DeserializeObject<List<BookDTO>>(stringur.Result);
            });
            getAfterEditedBackResponse.Wait();

            Assert.AreNotEqual(booksAfterEditedBackLoan.Count, nrOfBooksOnLoanAfterEdit);
            Assert.AreEqual(booksAfterEditedBackLoan.Count, nrOfBooksOnLoanAfterEdit + 1);

            //Delete loan
            var deleteResponse = client.DeleteAsync($"/api/v1/users/{user_id}/books/{book_id}");
            deleteResponse.Wait();
        }

        #endregion

        #region FailingPathsUsersBooks
        

        /// <summary>
        /// Fail test if book is already loaned
        /// path: /api/v1/users/{user_id}/books/{book_id}
        /// method: PUT
        /// </summary>
        [TestMethod]
        public void AddBookAgainOnLoanByUser()
        {
            Random r = new Random();
            int userInt = r.Next(1, 100); 
            int bookInt = r.Next(1,1000);

            int user_id = userInt;
            int book_id = bookInt;

            String today = DateTime.Now.ToString("yyyy-MM-dd");

            UserLoan newLoan = new UserLoan {
                LoanDate = today,
                ReturnedDate = ""
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(newLoan), Encoding.UTF8, "application/json");
            string message = "";
            var postResponse = client.PostAsync($"/api/v1/users/{user_id}/books/{book_id}", stringContent)            
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                message = stringur.Result;
            });
            postResponse.Wait();
            Assert.IsTrue(postResponse.IsCompletedSuccessfully);

            UserLoan newLoanAgain = new UserLoan {
                LoanDate = today,
                ReturnedDate = ""
            };

            var stringContent2 = new StringContent(JsonConvert.SerializeObject(newLoanAgain), Encoding.UTF8, "application/json");
            string message2 = "";
            var postResponse2 = client.PostAsync($"/api/v1/users/{user_id}/books/{book_id}", stringContent2)            
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                message2 = stringur.Result;
            });
            postResponse2.Wait();
            Assert.AreEqual(message2, "Book already loaned to user");
            Assert.IsTrue(postResponse2.IsCompletedSuccessfully);

            //Delete loan
            var deleteResponse = client.DeleteAsync($"/api/v1/users/{user_id}/books/{book_id}")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                message2 = stringur.Result;
            });
            deleteResponse.Wait();
        }

        /// <summary>
        /// Fail test if book is already passed loandate 
        /// and tries to delete.
        /// path: /api/v1/users/{user_id}/books/{book_id}
        /// method: PUT
        /// </summary>
    	[TestMethod]
        public void DeleteBookOnLoanByUserWithReturnDateInPast()
        {
            Random r = new Random();
            int userInt = r.Next(1, 100); 
            int bookInt = r.Next(1,1000);

            int user_id = userInt;
            int book_id = bookInt;

            String today = DateTime.Now.ToString("yyyy-MM-dd");

            UserLoan newLoan = new UserLoan {
                LoanDate = today,
                ReturnedDate = ""
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(newLoan), Encoding.UTF8, "application/json");
            string message = "";
            var postResponse = client.PostAsync($"/api/v1/users/{user_id}/books/{book_id}", stringContent)            
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                message = stringur.Result;
            });
            postResponse.Wait();
            Assert.IsTrue(postResponse.IsCompletedSuccessfully);

            UserLoan newLoanAgainWithReturnDate = new UserLoan {
                LoanDate = today,
                ReturnedDate = today
            };

            var stringContent2 = new StringContent(JsonConvert.SerializeObject(newLoanAgainWithReturnDate), Encoding.UTF8, "application/json");
            
            var postResponse2 = client.PutAsync($"/api/v1/users/{user_id}/books/{book_id}", stringContent2)            
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
            });
            postResponse2.Wait();
            Assert.IsTrue(postResponse2.IsCompletedSuccessfully);
            string message2 = "";
            //Delete loan
            var deleteResponse = client.DeleteAsync($"/api/v1/users/{user_id}/books/{book_id}")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                message2 = stringur.Result;
            });
            deleteResponse.Wait();
            Assert.IsTrue(deleteResponse.IsCompletedSuccessfully);
            Assert.AreEqual(message2, "Book not in loan by user");
        }

        #endregion

		#endregion

		#region Users -> Reviews    

        #region HappyPathUsersReviews


        /// <summary>
        /// Get all reviews from users
        /// path: /api/v1/users/{user_id}/books/{book_id}
        /// method: POST
        /// </summary>
        [TestMethod]
        public void getAllReviewsForUser()
        {
            int user_id = 90;
            List<ReviewsDTO> review = null;
            var response = client.GetAsync($"/api/v1/users/{user_id}/reviews")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var ret = res.Content.ReadAsStringAsync();
                ret.Wait();
                review = JsonConvert.DeserializeObject<List<ReviewsDTO>>(ret.Result);
            });
            response.Wait();
            Assert.IsTrue(response.IsCompletedSuccessfully);
            Assert.IsNotNull(review);
            Assert.AreEqual(review.Count, 2);
        }


        /// <summary>
        /// Get review that user has posted
        /// path: /api/v1/users/{user_id}/reviews/{book_id}
        /// method: GET
        /// </summary>
        [TestMethod]
        public void getReviewFromBook()
        {
            int user_id = 90;
            int book_id = 788;
            ReviewsDTO review = null;
            var response = client.GetAsync($"/api/v1/users/{user_id}/reviews/{book_id}")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var ret = res.Content.ReadAsStringAsync();
                ret.Wait();
                review = JsonConvert.DeserializeObject<ReviewsDTO>(ret.Result);
            });
            response.Wait();
            Assert.IsTrue(response.IsCompletedSuccessfully);
            Assert.IsNotNull(review);
            Assert.AreEqual(review.Stars, 2);
            Assert.AreEqual(review.Review, "Did not enjoy this book.");
        }


        /// <summary>
        /// Adds review to book from user
        /// path: /api/v1/users/{user_id}/reviews/{book_id}
        /// method: POST
        /// </summary>
        [TestMethod]
        public void postReviewToBookFromUser()
        {
            int user_id = 50;
            int book_id = 50;
            ReviewsDTO RetReview = null;
            ReviewViewModel PostRev = new ReviewViewModel{
                Stars = 5,
                UserReview = "this book was great"
            };

            var stringContent1 = new StringContent(JsonConvert
            .SerializeObject(PostRev), Encoding.UTF8, "application/json");

            var response = client.PostAsync($"/api/v1/users/{user_id}/reviews/{book_id}", stringContent1)
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                RetReview = JsonConvert.DeserializeObject<ReviewsDTO>(stringur.Result);
            });
            response.Wait();

            var delete = client.DeleteAsync($"/api/v1/users/{user_id}/reviews/{book_id}");
            delete.Wait();
            Assert.IsTrue(delete.IsCompletedSuccessfully);
            Assert.IsTrue(response.IsCompletedSuccessfully);
            Assert.IsNotNull(RetReview);
            Assert.AreEqual(PostRev.Stars, RetReview.Stars);
            Assert.AreEqual(PostRev.UserReview, RetReview.Review);
        }

        /// <summary>
        /// Removes a review from book that user has posed.
        /// path: /api/v1/users/{user_id}/reviews/{book_id}
        /// method: DELETE
        /// </summary>
        [TestMethod]
        public void DeleteReviewToBookFromUser()
        {
            int user_id = 50;
            int book_id = 500;
            ReviewsDTO RetReview = null;
            ReviewViewModel PostRev = new ReviewViewModel{
                Stars = 5,
                UserReview = "this book was great"
            };

            var stringContent1 = new StringContent(JsonConvert
            .SerializeObject(PostRev), Encoding.UTF8, "application/json");

            var response = client.PostAsync($"/api/v1/users/{user_id}/reviews/{book_id}", stringContent1)
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                RetReview = JsonConvert.DeserializeObject<ReviewsDTO>(stringur.Result);
            });
            response.Wait();

            var delete = client.DeleteAsync($"/api/v1/users/{user_id}/reviews/{book_id}");
            delete.Wait();
            Assert.IsTrue(delete.IsCompletedSuccessfully);
            Assert.IsTrue(response.IsCompletedSuccessfully);
            Assert.IsNotNull(RetReview);
            Assert.AreEqual(PostRev.Stars, RetReview.Stars);
            Assert.AreEqual(PostRev.UserReview, RetReview.Review);
        }

        /// <summary>
        /// Edit a review that for book that user has posted.
        /// path: /api/v1/users/{user_id}/reviews/{book_id}
        /// method: PUT
        /// </summary>
        [TestMethod]
        public async Task PutReviewToBookFromUser()
        {
            int user_id = 50;
            int book_id = 500;
            ReviewViewModel PostRev = new ReviewViewModel{
                Stars = 5,
                UserReview = "this book was great"
            };
            ReviewViewModel PutReview = new ReviewViewModel{
                Stars = 3,
                UserReview = "This was a mistakse."
            };

            var stringContent1 = new StringContent(JsonConvert
            .SerializeObject(PostRev), Encoding.UTF8, "application/json");

            var  stringContent2 = new StringContent(JsonConvert
            .SerializeObject(PutReview), Encoding.UTF8, "application/json");

            var PostRes = await client.PostAsync($"/api/v1/users/{user_id}/reviews/{book_id}", stringContent1);

            var PutRes = await client.PutAsync($"/api/v1/users/{user_id}/reviews/{book_id}", stringContent2);

            var delete = await client.DeleteAsync($"/api/v1/users/{user_id}/reviews/{book_id}");
            Assert.IsTrue(delete.IsSuccessStatusCode);
            Assert.IsTrue(PostRes.IsSuccessStatusCode);
            Assert.IsTrue(PutRes.IsSuccessStatusCode);
        }

        #endregion

        #region FailingPathsUsersReviews

        //Delete a review that doesn't exist


        /// <summary>
        /// FailTest tries to remove review that does not exists.
        /// path: /api/v1/users/{user_id}/reviews/{book_id}
        /// method: DELETE
        /// </summary>
        [TestMethod]
        public async Task DeleteReviewThatDoesNotExists()
        {
            ReviewViewModel PostRev = new ReviewViewModel{
                Stars = 5,
                UserReview = "this book was great"
            };
            int book_id = -1;
            int user_id = -1;
            var stringContent1 = new StringContent(JsonConvert
            .SerializeObject(PostRev), Encoding.UTF8, "application/json");

            var delete = await client.DeleteAsync($"/api/v1/users/{user_id}/reviews/{book_id}");
            Assert.IsFalse(delete.IsSuccessStatusCode);
        }

        //Put a review with no stars and userreview


        /// <summary>
        /// Fail test, tries to add review with no content
        /// path: /api/v1/users/{user_id}/reviews/{book_id}
        /// method: PUT
        /// </summary>
        [TestMethod]
        public async Task putReviewWithNoStarsFailing()
        {
            ReviewViewModel PostRev = new ReviewViewModel{
                Stars = 5,
                UserReview = "this book was great"
            };
            ReviewViewModel PutReview = new ReviewViewModel{
            };
            int user_id = 50;
            int book_id = 500;
            var stringContent1 = new StringContent(JsonConvert
            .SerializeObject(PostRev), Encoding.UTF8, "application/json");

            var  stringContent2 = new StringContent(JsonConvert
            .SerializeObject(PutReview), Encoding.UTF8, "application/json");

            var PostRes = await client.PostAsync($"/api/v1/users/{user_id}/reviews/{book_id}", stringContent1);

            var PutRes = await client.PutAsync($"/api/v1/users/{user_id}/reviews/{book_id}", stringContent2);

            var delete = await client.DeleteAsync($"/api/v1/users/{user_id}/reviews/{book_id}");
            Assert.IsTrue(delete.IsSuccessStatusCode);
            Assert.IsTrue(PostRes.IsSuccessStatusCode);
            Assert.IsFalse(PutRes.IsSuccessStatusCode);
            
        }

        #endregion

		#endregion

		#region Users -> Recommendations    

        #region HappyPathUsersRecommendations

        /// <summary>
        /// Get recommended books for user
        /// path: /api/v1/users/{user_id}/recommendation
        /// method: GET
        /// </summary>
        [TestMethod]
        public void GetBookRecommendationsForUser()
        {
            List<BookDTO> recommendedBooks = null;
            int user_id = 90;
            var response = client.GetAsync($"/api/v1/users/{user_id}/recommendation")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var stringur = res.Content.ReadAsStringAsync();
                stringur.Wait();
                recommendedBooks = JsonConvert.DeserializeObject<List<BookDTO>>(stringur.Result);

            });
            response.Wait();

            //Check to see if list contains 5 books
            Assert.IsNotNull(recommendedBooks);
            Assert.AreEqual(recommendedBooks.Count, 5);
        }
        
        #endregion

        #region FailingPathsUsersRecommendations

        //Get recommendations for user that has read all books

        #endregion

		#endregion

		#region Books -> Reviews    

        #region HappyPathBooksReviews


        /// <summary>
        /// Get a review for book and user that has written.
        /// path: /api/v1/books/{book_id}/reviews/{user_id}
        /// method: GET
        /// </summary>
        [TestMethod]
        public void GetReviewByBookAndId()
        {
            int book_id = 797;
            int user_id = 3;
            ReviewsDTO review = null;
            var response = client.GetAsync($"/api/v1/books/{book_id}/reviews/{user_id}")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var ret = res.Content.ReadAsStringAsync();
                ret.Wait();
                review = JsonConvert.DeserializeObject<ReviewsDTO>(ret.Result);
            });
            response.Wait();
            Assert.IsNotNull(review);
            Assert.AreEqual(review.UserName, "Pierette Klawi");
        }

        /// <summary>
        /// Edits a review from book that user has written.
        /// path: /api/v1/books/{book_id}/reviews/{user_id}
        /// method: PUT
        /// </summary>
        [TestMethod]
        public async Task EditReviewByBookAndId()
        {
            ReviewViewModel PostRev = new ReviewViewModel{
                Stars = 5,
                UserReview = "this book was great"
            };
            ReviewViewModel PutReview = new ReviewViewModel{
                Stars = 3,
                UserReview = "This was a mistakse."
            };

            int book_id = 1;
            int user_id = 1;

            var stringContent1 = new StringContent(JsonConvert
            .SerializeObject(PostRev), Encoding.UTF8, "application/json");

            var  stringContent2 = new StringContent(JsonConvert
            .SerializeObject(PutReview), Encoding.UTF8, "application/json");

            var PostRes = await client.PostAsync($"/api/v1/users/{user_id}/reviews/{book_id}", stringContent1);

            var PutRes = await client.PutAsync($"/api/v1/books/{book_id}/reviews/{user_id}", stringContent2);

            var delete = await client.DeleteAsync($"/api/v1/users/{user_id}/reviews/{book_id}");
            Assert.IsTrue(delete.IsSuccessStatusCode);
            Assert.IsTrue(PostRes.IsSuccessStatusCode);
            Assert.IsTrue(PutRes.IsSuccessStatusCode);

        }

        /// <summary>
        /// Deletes a Review from book that user has written.
        /// path: /api/v1/users/{user_id}/reviews/{book_id}
        /// method: PUT
        /// </summary>
        [TestMethod]
        public async Task DeleteReviewByBook()
        {
            ReviewViewModel PostRev = new ReviewViewModel{
                Stars = 5,
                UserReview = "this book was great"
            };
            int book_id = 2;
            int user_id = 2;
            var stringContent1 = new StringContent(JsonConvert
            .SerializeObject(PostRev), Encoding.UTF8, "application/json");

            var PostRes = await client.PostAsync($"/api/v1/users/{user_id}/reviews/{book_id}", stringContent1);

            var delete = await client.DeleteAsync($"/api/v1/users/{user_id}/reviews/{book_id}");
            Assert.IsTrue(delete.IsSuccessStatusCode);
            Assert.IsTrue(PostRes.IsSuccessStatusCode);
        }

        /// <summary>
        /// Gets all reviews from database.
        /// path: /api/v1/books/reviews
        /// method: GET
        /// </summary>
        [TestMethod]
        public void getAllReviews()
        {
            List<ReviewsDTO> review = null;
            var response = client.GetAsync("/api/v1/books/reviews")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var ret = res.Content.ReadAsStringAsync();
                ret.Wait();
                review = JsonConvert.DeserializeObject<List<ReviewsDTO>>(ret.Result);
 
            
            });
            response.Wait();
            Assert.IsTrue(response.IsCompletedSuccessfully);
            Assert.IsNotNull(review);
            Assert.AreEqual(review.Count, 10);
            
        }
        
        /// <summary>
        /// Gets all reviews from specific book in database.
        /// path: /api/v1/books/{book_id}/reviews
        /// method: GET
        /// </summary>
        [TestMethod]
        public void getAllReviewsForBook()
        {
            List<ReviewsDTO> review = null;
            int book_id = 413;
            var response = client.GetAsync($"/api/v1/books/{book_id}/reviews")
            .ContinueWith((taskResponse) =>
            {
                var res = taskResponse.Result;
                var ret = res.Content.ReadAsStringAsync();
                ret.Wait();
                review = JsonConvert.DeserializeObject<List<ReviewsDTO>>(ret.Result);
            });
            response.Wait();
            Assert.IsTrue(response.IsCompletedSuccessfully);
            Assert.IsNotNull(review);
            Assert.AreEqual(review.Count, 2);
        }
        #endregion

        #region FailingPathBooksReviews

        /// <summary>
        /// Fail Test that tries to delete review that does
        /// not exist.
        /// path: /api/v1/books/{book_id}/reviews/{user_id}
        /// method: DELETE
        /// </summary>
        [TestMethod]
        public async Task DeleteBookReviewThatDoesNotExists()
        {
        
            ReviewViewModel PostRev = new ReviewViewModel{
                Stars = 5,
                UserReview = "this book was great"
            };
            int book_id = -1;
            int user_id = -1;
            var stringContent1 = new StringContent(JsonConvert
            .SerializeObject(PostRev), Encoding.UTF8, "application/json");

            var delete = await client.DeleteAsync($"/api/v1/books/{book_id}/reviews/{user_id}");
            Assert.IsFalse(delete.IsSuccessStatusCode);

        }


        /// <summary>
        /// Fail Test that tries to edit review with no 
        /// stars or review.
        /// not exist.
        /// path: /api/v1/books/{book_id}/reviews/{user_id}
        /// method: PUT
        /// </summary>
        [TestMethod]
        public async Task putReviewWithNoStarsFailingBooks()
        {
            ReviewViewModel PostRev = new ReviewViewModel{
                Stars = 5,
                UserReview = "this book was great"
            };
            ReviewViewModel PutReview = new ReviewViewModel{
            };
            int user_id = 50;
            int book_id = 500;
            var stringContent1 = new StringContent(JsonConvert
            .SerializeObject(PostRev), Encoding.UTF8, "application/json");

            var  stringContent2 = new StringContent(JsonConvert
            .SerializeObject(PutReview), Encoding.UTF8, "application/json");

            var PostRes = await client.PostAsync($"/api/v1/users/{user_id}/reviews/{book_id}", stringContent1);

            var PutRes = await client.PutAsync($"/api/v1/books/{book_id}/reviews/{user_id}", stringContent2);

            var delete = await client.DeleteAsync($"/api/v1/users/{user_id}/reviews/{book_id}");
            Assert.IsTrue(delete.IsSuccessStatusCode);
            Assert.IsTrue(PostRes.IsSuccessStatusCode);
            Assert.IsFalse(PutRes.IsSuccessStatusCode);
        
        }
        #endregion

		#endregion
    }
}