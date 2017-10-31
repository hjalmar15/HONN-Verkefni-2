using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BookApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BookApi.Repositories
{
    public static class LoadFiles
    {    
        public static void Initialize(IServiceProvider serviceProvider)
        {
        using (var context = new AppDataContext(
                    serviceProvider.GetRequiredService<DbContextOptions<AppDataContext>>()))
                    {
                        if(context.Users.Any()){
                            return; 
                            //Db has been seeded;
                        }
                        using (StreamReader r = new StreamReader("../JsonFiles/Books.json"))
                        {
                            string json = r.ReadToEnd();
                            List<tempBook> items = JsonConvert.DeserializeObject<List<tempBook>>(json);
                            foreach (var Book in items)
                            {
                                Book book = new Book {
                                Id = Book.bok_id,
                                Title = Book.bok_titill,
                                Author = Book.fornafn_hofundar + " " + Book.eftirnafn_hofundar,
                                DatePublished = Book.utgafudagur,
                                ISBN = Book.ISBN
                                };
                                context.Books.Add(book);
                                context.SaveChanges();
                            }
                        };
                        using (StreamReader x = new StreamReader("../JsonFiles/Users.json"))
                        {
                            string json = x.ReadToEnd();
                            List<Users> items = JsonConvert.DeserializeObject<List<Users>>(json);
                            foreach (var User in items)
                            {
                                Console.WriteLine("added");
                                User userRepo = new User(){
                                Id = User.vinur_id,
                                Name = User.fornafn + " " + User.eftirnafn,
                                Address = User.heimilisfang,
                                Email = User.netfang,
                            };
                                context.Users.Add(userRepo);
                                context.SaveChanges();
                                if (User.lanasafn != null)
                                {

                                    Console.WriteLine("lanasafn added");
                                    foreach(var lanasafn in User.lanasafn)
                                    {
                                        UserBook safn = new UserBook(){
                                        UserId = User.vinur_id,
                                        BookId = lanasafn.bok_id,
                                        LoanDate = Convert.ToDateTime(lanasafn.bok_lanadagsetning),
                                    };
                                    context.UserBooks.Add(safn);
                                    context.SaveChanges();
                                    }
                                }
                     
                            
                
                            }
                        };
                    }
        }
        public class tempBook
        {
            public int bok_id { get; set; }
            public string bok_titill { get; set; }
            public string fornafn_hofundar { get; set; }
            public string eftirnafn_hofundar { get; set; }
            public string utgafudagur { get; set; }
            public string ISBN { get; set; }
        }
        public class Users
        {
            public int vinur_id { get; set; }
            public string fornafn { get; set; }
            public string eftirnafn { get; set; }
            public string heimilisfang { get; set; }
            public string netfang { get; set; }
            public System.Collections.ObjectModel.Collection<lanasafn> lanasafn { get; set;}
        }

        public class lanasafn
        {
            public int bok_id { get; set; }
            public DateTime bok_lanadagsetning { get; set; }
            public int vinur_id { get; set; }
        }

    }           
}
