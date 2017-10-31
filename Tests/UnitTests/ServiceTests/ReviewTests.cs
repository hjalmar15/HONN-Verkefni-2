
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
    public class ReviewTests
    {
        private IEnumerable<ReviewsDTO> _Reviews;
        private Mock<IRepositoryService> _mockRepo;

        [TestInitialize]
        public void ReviewTestsSetup()
        {
            var _ReviewsDTO = new List<ReviewsDTO>();
            _ReviewsDTO.Add(new ReviewsDTO {
                BookName = "The Glory",
                UserName = "Jon Jonsson",
                Stars = 2,
                Review = "I hate this book"

            });
            _ReviewsDTO.Add(new ReviewsDTO{
                BookName = "How to be mom",
                UserName = "Jon Jonsson",
                Stars = 3,
                Review = "This book was fine"
            });
            _ReviewsDTO.Add(new ReviewsDTO{
                BookName = "How to ask someone to marry them",
                UserName = "Jon Jonsson",
                Stars = 2,
                Review = "Most useless book ive read"
            });

             _Reviews = _ReviewsDTO as IEnumerable<ReviewsDTO>;
             _mockRepo = new Mock<IRepositoryService>();
        }
        [TestMethod]
        public void getBookReview()
        {
            _mockRepo.Setup(x => x.getBookReview()).Returns(_Reviews);
            var ReviewServices = new ReviewService(_mockRepo.Object);
            var reviewscount = ReviewServices.getBookReview();
            ReviewServices.Should().NotBeNull();
            reviewscount.Should().HaveCount(3);

            
        }
        [TestMethod]
        public void getBookReviewById()
        {
            _mockRepo.Setup(x => x.getBookReviewById(1)).Returns(_Reviews);
            var ReviewServices = new ReviewService(_mockRepo.Object);
            var reviewscount = ReviewServices.getBookReviewById(1);
            ReviewServices.Should().NotBeNull();
            reviewscount.Should().HaveCount(3);
        }
        [TestMethod]
        public void getBookReviewFromUser()
        {
            var testReview = new ReviewsDTO{
                BookName = "The glorious book of glory",
                UserName = "Jon Jonsson",
                Stars = 3,
                Review = "this is a best book ive read"
            };
            _mockRepo.Setup(x => x.getBookReviewFromUser(1,1)).Returns(testReview);
            var ReviewServices = new ReviewService(_mockRepo.Object);
            var reviewscount = ReviewServices.getBookReviewFromUser(1,1);
            ReviewServices.Should().NotBeNull();
            reviewscount.BookName.Should().BeEquivalentTo("The glorious book of glory");
            reviewscount.Stars.Should().BeLessOrEqualTo(3);
            ReviewServices.Should().NotBeNull();
        
        }
        
        [TestMethod]
        public void postBookReviewFromUser()
        {
            var testReview = new ReviewsDTO{
                BookName = "The glorious book of glory",
                UserName = "Jon Jonsson",
                Stars = 3,
                Review = "this is a best book ive read"
            };
            var reviewModel = new ReviewViewModel{
                Stars = 3,
                UserReview = "this is a best book ive read"
            };
            _mockRepo.Setup(x => x.addBookReviewFromUser(1,1, reviewModel)).Returns(testReview);
            var ReviewServices = new ReviewService(_mockRepo.Object);
            var reviewscount = ReviewServices.addBookReviewFromUser(1,1, reviewModel);
            reviewscount.BookName.Should().BeEquivalentTo("The glorious book of glory");
            reviewscount.Stars.Should().BeGreaterThan(2);
            ReviewServices.Should().NotBeNull();
       
        }

        [TestMethod]
        public void editBookReviewFromUser()
        {
            var testReview = new ReviewsDTO{
                BookName = "The glorious book of glory",
                UserName = "Jon Jonsson",
                Stars = 3,
                Review = "this is a best book ive read"
            };
            var reviewModel = new ReviewViewModel{
                Stars = 3,
                UserReview = "The edited Text"
            };
            _mockRepo.Setup(x => x.editBookReviewFromUser(1,1, reviewModel )).Returns(testReview);
            var ReviewServices = new ReviewService(_mockRepo.Object);
            var reviewscount = ReviewServices.editBookReviewFromUser(1,1, reviewModel);
            reviewscount.BookName.Should().BeEquivalentTo("The glorious book of glory");
            reviewscount.Stars.Should().BeGreaterThan(2);
            ReviewServices.Should().NotBeNull();
       
        }
        [TestMethod]
        public void deleteReviewFromUser()
        {
            var testReview = new ReviewsDTO{
                BookName = "The glorious book of glory",
                UserName = "Jon Jonsson",
                Stars = 3,
                Review = "this is a best book ive read"
                };
            _mockRepo.Setup(x => x.deleteReviewFromUser(1,1)).Returns(testReview);
            var ReviewServices = new ReviewService(_mockRepo.Object);
            var reviewscount = ReviewServices.deleteReviewFromUser(1,1);
            reviewscount.BookName.Should().BeEquivalentTo("The glorious book of glory");
            reviewscount.Stars.Should().BeLessOrEqualTo(3);
            ReviewServices.Should().NotBeNull();


        }
        [TestMethod]
        public void getReviewsById()
        {
            _mockRepo.Setup(x => x.getReviewsById(1)).Returns(_Reviews);
            var ReviewServices = new ReviewService(_mockRepo.Object);
            var reviewscount = ReviewServices.getReviewsById(1);
            ReviewServices.Should().NotBeNull();
            reviewscount.Should().HaveCount(3);
        }
        [TestMethod]
        public void getReviewById()
        {
            var Review = new ReviewsDTO{
                BookName = "Some Glory of Book",
                UserName = "Jon Jonsson",
                Stars = 3,
                Review = "This is best book ive read"
            };
           _mockRepo.Setup(x => x.getReviewById(1,1)).Returns(Review);
            var ReviewServices = new ReviewService(_mockRepo.Object);
            var reviewscount = ReviewServices.getReviewById(1,1);
            reviewscount.BookName.Should().BeEquivalentTo("Some Glory of Book");
            reviewscount.Stars.Should().BeLessOrEqualTo(3);
            ReviewServices.Should().NotBeNull();
        }

    }
}