﻿// <auto-generated />
using BookApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Api.Migrations
{
    [DbContext(typeof(AppDataContext))]
    partial class AppDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("BookApi.Models.Entities.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("DatePublished");

                    b.Property<string>("ISBN");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("BookApi.Models.Entities.Review", b =>
                {
                    b.Property<int>("ReviewID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BookId");

                    b.Property<int>("Stars");

                    b.Property<int>("UserId");

                    b.Property<string>("UserReview");

                    b.HasKey("ReviewID");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("BookApi.Models.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BookApi.Models.Entities.UserBook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BookId");

                    b.Property<DateTime>("LoanDate");

                    b.Property<DateTime>("ReturnedDate");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserBooks");
                });
#pragma warning restore 612, 618
        }
    }
}