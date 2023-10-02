using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Web_153502_Logvinovich.Api.Data;
using Web_153502_Logvinovich.API.Services;
using Web_153502_Logvinovich.Controllers;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Domain.Models;
using Web_153502_Logvinovich.Services.AuthorService;
using Web_153502_Logvinovich.Services.BookService;
using Xunit.Abstractions;

namespace Web_153502_Logvinovich.Tests
{
    public class ApiBookServiceTests: IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<ApplicationDbContext> _options;
        
        private readonly ITestOutputHelper output;
        ApplicationDbContext CreateContext() => new ApplicationDbContext(_options);
        
        public ApiBookServiceTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseSqlite(_connection)
              .Options;


            using var context = CreateContext();

            context.Database.EnsureCreated();
            var authors = new List<Author>()
            {
                new Author { Name = "Лев Толстой", NormalizedName = "lev-tolstoy" },
                new Author { Name = "Александр Пушкин", NormalizedName = "aleksandr-pushkin" },
                new Author { Name = "Фёдор Достоевский", NormalizedName = "fyodor-dostoevsky" },
                new Author { Name = "Иван Тургенев", NormalizedName = "ivan-turgenev" },
                new Author { Name = "Николай Гоголь", NormalizedName = "nikolay-gogol" },
                new Author { Name = "Михаил Булгаков", NormalizedName = "mikhail-bulgakov" }
            };
            context.AddRange(authors);

            var books = new List<Book>
            {
                new Book { Name = "Война и мир", Author = authors[0], Description = "Эпический роман о жизни русского общества в эпоху Наполеоновских войн", Image = "{url}/images/book1.jpg", Price = 1000 },
                new Book { Name = "Война и мир", Author = authors[0], Description = "Эпический роман о жизни русского общества в эпоху Наполеоновских войн", Image = "{url}/images/book1.jpg", Price = 1000 },
                new Book { Name = "Евгений Онегин", Author = authors[1], Description = "Роман в стихах о любви и судьбе двух главных героев - Онегина и Татьяны", Image = "{url}/images/book2.jpg", Price = 500 },
                new Book { Name = "Преступление и наказание", Author = authors[2], Description = "Психологический роман о преступлении и его последствиях для совести главного героя - Родиона Раскольникова", Image = "{url}/images/book3.webp", Price = 800 },
                new Book { Name = "Отцы и дети", Author = authors[3], Description = "Роман о конфликте между поколениями в России середины XIX века", Image = "{url}/images/book4.jpg", Price = 600 },
                new Book { Name = "Мертвые души", Author = authors[4], Description = "Сатирический роман о похождениях мошенника Чичикова, который покупает у помещиков умерших крестьян - 'мертвые души'", Image = "{url}/images/book5.jpg", Price = 700 },
                new Book { Name = "Мастер и Маргарита", Author = authors[5], Description = "Фантастический роман о прибытии дьявола в Москву и его влиянии на жизнь нескольких персонажей, в том числе загадочного Мастера и его возлюбленной Маргариты", Image = "{url}/images/book6.jpg", Price = 900 }
            };

            context.AddRange(books);

            context.SaveChanges();
        }

        [Fact]
        public async void ServiceReturnsFirstPageOfThreeItems()
        {
            var context = CreateContext();
          
            var service = new BookService(context, null, null, null);
            var result = await service.GetBookListAsync("all");
            
            Assert.IsType<ResponseData<ListModel<Book>>>(result);
            Assert.True(result.Success);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(3, result.Data.TotalPages);
            Assert.Equal(context.Books.First(), result.Data.Items[0]);
        }


        [Fact]
        public async void ServicePicksSecondPage()
        {
            var context = CreateContext();

            var service = new BookService(context, null, null, null);
            var result = await service.GetBookListAsync("all", 2);
            
            Assert.IsType<ResponseData<ListModel<Book>>>(result);
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(3, result.Data.TotalPages);
        }

        [Fact]
        public async void ServiceFiltersBooksByAuthor()
        {
            var context = CreateContext();

            var service = new BookService(context, null, null, null);
            var result = await service.GetBookListAsync("lev-tolstoy", 1, 1);
            
            Assert.IsType<ResponseData<ListModel<Book>>>(result);
            Assert.True(result.Success);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Single(result.Data.Items);
            Assert.Equal(2, result.Data.TotalPages);
        }

        [Fact]
        public async void ServiceReturnsFalseWhenSetBigPageNumber()
        {
            var context = CreateContext();

            var service = new BookService(context, null, null, null);
            var result = await service.GetBookListAsync("all", 5, 25);
            
            Assert.IsType<ResponseData<ListModel<Book>>>(result);
            Assert.False(result.Success);
        }

        [Fact]
        public async void ServiceNotAllowSetBigPageSize()
        {
            var context = CreateContext();

            var author = new Author() { Name = "name", NormalizedName = "name" };
            var books = new List<Book>
            {
                new Book { Name = "Война и мир", Author = author, Description = "Эпический роман о жизни русского общества в эпоху Наполеоновских войн", Image = "{url}/images/book1.jpg", Price = 1000 },
                new Book { Name = "Война и мир", Author = author, Description = "Эпический роман о жизни русского общества в эпоху Наполеоновских войн", Image = "{url}/images/book1.jpg", Price = 1000 },
                new Book { Name = "Евгений Онегин", Author = author, Description = "Роман в стихах о любви и судьбе двух главных героев - Онегина и Татьяны", Image = "{url}/images/book2.jpg", Price = 500 },
                new Book { Name = "Преступление и наказание", Author = author, Description = "Психологический роман о преступлении и его последствиях для совести главного героя - Родиона Раскольникова", Image = "{url}/images/book3.webp", Price = 800 },
                new Book { Name = "Отцы и дети", Author = author, Description = "Роман о конфликте между поколениями в России середины XIX века", Image = "{url}/images/book4.jpg", Price = 600 },
                new Book { Name = "Мертвые души", Author = author, Description = "Сатирический роман о похождениях мошенника Чичикова, который покупает у помещиков умерших крестьян - 'мертвые души'", Image = "{url}/images/book5.jpg", Price = 700 },
                new Book { Name = "Мастер и Маргарита", Author = author, Description = "Фантастический роман о прибытии дьявола в Москву и его влиянии на жизнь нескольких персонажей, в том числе загадочного Мастера и его возлюбленной Маргариты", Image = "{url}/images/book6.jpg", Price = 900 },
                new Book { Name = "Война и мир", Author = author, Description = "Эпический роман о жизни русского общества в эпоху Наполеоновских войн", Image = "{url}/images/book1.jpg", Price = 1000 },
                new Book { Name = "Война и мир", Author = author, Description = "Эпический роман о жизни русского общества в эпоху Наполеоновских войн", Image = "{url}/images/book1.jpg", Price = 1000 },
                new Book { Name = "Евгений Онегин", Author = author, Description = "Роман в стихах о любви и судьбе двух главных героев - Онегина и Татьяны", Image = "{url}/images/book2.jpg", Price = 500 },
                new Book { Name = "Преступление и наказание", Author = author, Description = "Психологический роман о преступлении и его последствиях для совести главного героя - Родиона Раскольникова", Image = "{url}/images/book3.webp", Price = 800 },
                new Book { Name = "Отцы и дети", Author = author, Description = "Роман о конфликте между поколениями в России середины XIX века", Image = "{url}/images/book4.jpg", Price = 600 },
                new Book { Name = "Мертвые души", Author = author, Description = "Сатирический роман о похождениях мошенника Чичикова, который покупает у помещиков умерших крестьян - 'мертвые души'", Image = "{url}/images/book5.jpg", Price = 700 },
                new Book { Name = "Мастер и Маргарита", Author = author, Description = "Фантастический роман о прибытии дьявола в Москву и его влиянии на жизнь нескольких персонажей, в том числе загадочного Мастера и его возлюбленной Маргариты", Image = "{url}/images/book6.jpg", Price = 900 },                
            };
            // add books up to 21
            context.AddRange(books);
            context.SaveChanges();

            var service = new BookService(context, null, null, null);
            var result = await service.GetBookListAsync("all", 1, 21);
            Assert.True(result.Success);
            Assert.Equal(1, result.Data.CurrentPage);
            // not 21 as we can expect (max page size 20 so we have 2 pages(20, 1) not one with 21
            Assert.Equal(20, result.Data.Items.Count); 
            Assert.Equal(2, result.Data.TotalPages);
            
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
