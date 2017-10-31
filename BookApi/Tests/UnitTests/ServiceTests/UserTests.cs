 using System;
using System.Collections.Generic;
using BookApi.Repositories;
using BookApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentAssertions;
using BookApi.Models.Entities;
using BookApi.Models.DTOModels;
using BookApi.Models.ViewModels;

namespace BookApi.Tests
{
    [TestClass]
    public class UserTests
    {
        IEnumerable<CreatedUser> _UsersList;
        Mock<IRepositoryService> _mockRepo;
        UserService UserServices;
        ///<summary>
        /// Initilizes some values into the list and sets the mockRepo
        ///</summary>
        [TestInitialize]
        public void TestInitialize()
        {
            List<CreatedUser> Users = new List<CreatedUser>{
                new CreatedUser{
                    Id = 1,
                    Name = "Arnar Jó",
                    Address = "Hlunnó 15",
                    Email = "arnarjoh15@ru.is",
                    PhoneNumber = "7800372"
                },
                new CreatedUser{
                    Id = 2,
                    Name = "Jón Sigmunds",
                    Address = "Kirrakri 15",
                    Email = "jonhei15@ru.is",
                    PhoneNumber = "5812345"
                },
                new CreatedUser{
                    Id = 3,
                    Name = "Hjálmar Diego",
                    Address = "Ljósabrú 15",
                    Email = "hjalmar15@ru.is",
                    PhoneNumber = "5801515"
                }
            };
            _UsersList = Users as IEnumerable<CreatedUser>;

            _mockRepo = new Mock<IRepositoryService>();
        }

        ///<summary>
        /// Gets all users
        ///</summary>
        [TestMethod]
        public void getAllUsers()
        {
            _mockRepo.Setup(x => x.getUser(null, null)).Returns(_UsersList);
            UserServices = new UserService(_mockRepo.Object);
            var users = UserServices.getUser(null, null);
            UserServices.Should().NotBeNull();
            users.Should().HaveCount(3);
        }

        ///<summary>
        /// Gets a user from an ID
        ///</summary>
        [TestMethod]
        public void getSpecificUser()
        {
            var retUser = new UserAndLoansDTO{
                Id = 1,
                Name = "Arnar Jó",
                Address = "Hlunnó 15",
                Email = "arnarjoh15@ru.is",
                PhoneNumber = "7800372"
            };
            _mockRepo.Setup(x => x.getUserById(1)).Returns(retUser);
            UserServices = new UserService(_mockRepo.Object);
            var user = UserServices.getUserById(1);
            user.Should().NotBeNull();
            user.Should().Equals(retUser);
        }

        ///<summary>
        /// Posts a user to the list
        ///</summary>
        [TestMethod]
        public void postAUser()
        {
            var _newUser = new CreateUser{
                Name = "Nýr Notandi",
                Address = "Hlunnó 15",
                Email = "Nýr@ru.is",
                PhoneNumber = "5812345"
            };
            var _createdUser = new CreatedUser{
                Id = 1,
                Name = "Nýr Notandi",
                Address = "Hlunnó 15",
                Email = "Nýr@ru.is",
                PhoneNumber = "5812345"
            };
            _mockRepo.Setup(x => x.addUser(_newUser)).Returns(_createdUser);
            UserServices = new UserService(_mockRepo.Object);
            var user = UserServices.addUser(_newUser);
            UserServices.Should().NotBeNull();
            user.Name.Should().BeEquivalentTo("Nýr Notandi");
            user.Address.Should().BeEquivalentTo("Hlunnó 15");
            user.Email.Should().BeEquivalentTo("Nýr@ru.is");
            user.PhoneNumber.Should().BeEquivalentTo("5812345");
            user.Should().NotBeNull();
        }

        ///<summary>
        /// Changes a user according to its ID 
        ///</summary>
        [TestMethod]
        public void changeSpecificUser()
        {
            var changedUser = new CreateUser{
                Name = "Sölmundur",
                Address = "Hlunnó 15",
                Email = "arnarjoh15@ru.is",
                PhoneNumber = "7800372"
            };
            var afterChange = new CreatedUser{
                Id = 1,
                Name = "Sölmundur",
                Address = "Hlunnó 15",
                Email = "arnarjoh15@ru.is",
                PhoneNumber = "7800372"
            };

            _mockRepo.Setup(x => x.editUser(1, changedUser)).Returns(afterChange);
            UserServices = new UserService(_mockRepo.Object);
            var edited = UserServices.editUser(1, changedUser);
            edited.Name.Should().BeEquivalentTo(afterChange.Name);
            edited.Address.Should().BeEquivalentTo(afterChange.Address);
            edited.Email.Should().BeEquivalentTo(afterChange.Email);
            edited.PhoneNumber.Should().BeEquivalentTo(afterChange.PhoneNumber);
        }

        ///<summary>
        /// Deletes a user to the list
        ///</summary>
        [TestMethod]
        public void deleteSpecificUser()
        {
            _mockRepo.Setup(x => x.deleteUser(1)).Returns(true);
            UserServices = new UserService(_mockRepo.Object);
            var deleted = UserServices.deleteUser(1);
            deleted.Should().BeTrue();
        }
    }
}