using Microsoft.AspNetCore.Mvc;
using Moq;
using Web_153502_Logvinovich.Controllers;
using Web_153502_Logvinovich.Domain.Entities;
using Web_153502_Logvinovich.Domain.Models;
using Web_153502_Logvinovich.Services.AuthorService;
using Web_153502_Logvinovich.Services.BookService;

namespace Web_153502_Logvinovich.Tests
{
    public class BookControllerTests
    {
        [Fact]
        public async Task Index_ReturnsNotFound_WhenAuthorListNotSuccessful()
        {
            // Arrange
            var authorService = new Mock<IAuthorService>();
            var bookService = new Mock<IBookService>();
            authorService.Setup(c => c.GetAuthorListAsync()).ReturnsAsync(new ResponseData<List<Author>> { Success = false, ErrorMessage = "Error" });
            var controller = new BookController(bookService.Object, authorService.Object);

            // Act
            var result = await controller.Index("all");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Error", ((NotFoundObjectResult)result).Value);
        }

        [Fact]
        public async Task Index_ReturnsNotFound_WhenBookListNotSuccessful()
        {
            // Arrange
            var categoryService = new Mock<IAuthorService>();
            var bookService = new Mock<IBookService>();

            categoryService.Setup(c => c.GetAuthorListAsync()).ReturnsAsync(new ResponseData<List<Author>> { Success = true, Data = new List<Author>() });
            bookService.Setup(p => p.GetBookListAsync("all", 1)).ReturnsAsync(new ResponseData<ListModel<Book>> { Success = false, ErrorMessage = "Error" });
            var controller = new BookController(bookService.Object, categoryService.Object);

            // Act
            var result = await controller.Index("all");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Error", ((NotFoundObjectResult)result).Value);
        }

        [Fact]
        public async Task Index_ReturnsView_WithCorrectData_WhenSuccessful()
        {
            // Arrange
            var authorList = new List<Author>
            {
                new Author { Id = 1, Name = "Category1", NormalizedName = "category1" },
                new Author { Id = 2, Name = "Category2", NormalizedName = "category2" }
            };
            var bookList = new List<Book>
            {
                new Book { Id = 1, Name = "Product1" },
                new Book { Id = 2, Name = "Product2" }
            };

            var authorService = new Mock<IAuthorService>();
            authorService.Setup(c => c.GetAuthorListAsync()).ReturnsAsync(new ResponseData<List<Author>> { Success = true, Data = authorList });

            var bookService = new Mock<IBookService>();
            bookService.Setup(p => p.GetBookListAsync("all", 1)).ReturnsAsync(new ResponseData<ListModel<Book>> { Success = true, Data = new ListModel<Book>() { Items = bookList } });

            var controller = new BookController(bookService.Object, authorService.Object);

            // Act
            var result = await controller.Index("all");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ListModel<Book>>(viewResult.Model);

            // Check ViewData
            var categoriesInViewData = viewResult.ViewData["authors"] as IEnumerable<Author>;
            var currentCategoryInViewData = viewResult.ViewData["currentAuthor"] as string;

            Assert.Equal(authorList, categoriesInViewData);
            Assert.Equal("Все", currentCategoryInViewData);

            // Check the model
            Assert.Equal(bookList, model.Items);
        }
    }
}