using Microsoft.EntityFrameworkCore;
using BookApi.Models.Entities;

namespace BookApi.Repositories
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {

        }


        public DbSet<Book> Books { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserBook> UserBooks { get; set; }
        /*

        public DbSet<CourseTemplate> CoursesTemplate { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<StudentRegistration> StudentRegistration { get; set; } 

        public DbSet<WaitingList> WaitingList { get; set; }
        */

    }

    /*dotnet add package Microsoft.EntityFrameworkCore */
}