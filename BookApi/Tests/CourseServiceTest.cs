using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookApi.Repositories;
using BookApi.Services;
using BookApi.Models.DTOModels;
using System.Collections.Generic;
using System.Linq;


namespace CoursesApi.Tests
{
    [TestClass]
    public class CourseServiceTest
    {
        // private IRepositoryService _Repo;
        // private IUserService _userService;

        // [TestInitialize]
        // public void Setup()
        // {
        //     _Repo = new MockRepository();
        //     _userService = new UserService(_Repo);
        // }

        // [TestMethod]
        // public void GetAListOfCourses()
        // {
        //     var listOfCourses = _userService.GetCoursesBySemester(null);
        //     Assert.IsNotNull(listOfCourses);
        // }
        // [TestMethod]
        // public void GetAListOfStudents()
        // {
        //     var theCourse = 1;
        //     List<StudentDTO> listOfStudents = _userService.GetStudents(theCourse).ToList();
        //     List<StudentDTO> expectedListOfStudents = new List<StudentDTO>
        //     {
        //         new StudentDTO {
        //             Name = "Arnar Jóhannsson",
        //             SSN = 0810952649
        //         },
        //         new StudentDTO {
        //             Name = "Hjálmar Diego Arnórsson",
        //             SSN = 2901952549
        //         }
        //     };
        //     var areEqual = false;
        //     if(listOfStudents.SequenceEqual(expectedListOfStudents))
        //     {
        //         areEqual = true;
        //     }
        //     Assert.IsTrue(areEqual);
        // }
    }
}
