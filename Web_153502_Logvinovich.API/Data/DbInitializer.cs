using Microsoft.EntityFrameworkCore;
using System;
using Web_153502_Logvinovich.Api.Data;
using Web_153502_Logvinovich.Domain.Entities;

namespace Web_153502_Logvinovich.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            await context.Database.MigrateAsync();
            
            
            var url = app.Configuration.GetValue<string>("Url");


            var authors = new List<Author>
            {
                new Author { Name = "Лев Толстой", NormalizedName =  "lev-tolstoy" },
                new Author { Name = "Александр Пушкин", NormalizedName = "aleksandr-pushkin" },
                new Author { Name = "Фёдор Достоевский", NormalizedName = "fyodor-dostoevsky" },
                new Author { Name = "Иван Тургенев", NormalizedName = "ivan-turgenev" },
                new Author { Name = "Николай Гоголь", NormalizedName = "nikolay-gogol" },
                new Author { Name = "Михаил Булгаков", NormalizedName = "mikhail-bulgakov" }
            };

            foreach (var author in authors)
            {
                if(!context.Authors.Any(a => a.NormalizedName == author.NormalizedName)) context.Authors.Add(author);
            }

            var books = new List<Book>
            {
                new Book { Name = "Война и мир", Author = authors[0], Description = "Эпический роман о жизни русского общества в эпоху Наполеоновских войн", Image = $"{url}/images/book1.jpg", Price = 1000 },
                new Book { Name = "Евгений Онегин", Author = authors[1], Description = "Роман в стихах о любви и судьбе двух главных героев - Онегина и Татьяны", Image = $"{url}/images/book2.jpg", Price = 500 },
                new Book { Name = "Преступление и наказание", Author = authors[2], Description = "Психологический роман о преступлении и его последствиях для совести главного героя - Родиона Раскольникова", Image = $"{url}/images/book3.webp", Price = 800 },
                new Book { Name = "Отцы и дети", Author = authors[3], Description = "Роман о конфликте между поколениями в России середины XIX века", Image = $"{url}/images/book4.jpg", Price = 600 },
                new Book { Name = "Мертвые души", Author = authors[4], Description = "Сатирический роман о похождениях мошенника Чичикова, который покупает у помещиков умерших крестьян - 'мертвые души'", Image = $"{url}/images/book5.jpg", Price = 700 },
                new Book { Name = "Мастер и Маргарита", Author = authors[5], Description = "Фантастический роман о прибытии дьявола в Москву и его влиянии на жизнь нескольких персонажей, в том числе загадочного Мастера и его возлюбленной Маргариты", Image = $"{url}/images/book6.jpg", Price = 900 }
            };



            foreach (var book in books)
            {
                if (!context.Books.Any(b => b.Name == book.Name))
                {
                    context.Books.Add(book);
                }
            }

            await context.SaveChangesAsync();
            
        }
    }
}
