using johns_lyng_app.Server.Context;
using johns_lyng_app.Server.Interface;
using johns_lyng_app.Server.Models;
using johns_lyng_app.Server.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace TodoList.Api.UnitTests
{
    public class TodoItemsServiceTests
    {
        private readonly TodoContext _context;
        private readonly Mock<ILogger<TodoItemsService>> _mockLogger;
        private readonly Mock<IConfiguration> _mockConfiguration; 

        private readonly ITodoItemsService _service;

        public TodoItemsServiceTests()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: "TodoListTest")
                .Options;

            _context = new TodoContext(options);
            _mockLogger = new Mock<ILogger<TodoItemsService>>(); 
            _mockConfiguration = new Mock<IConfiguration>();
            _service = new TodoItemsService(_context, _mockLogger.Object, _mockConfiguration.Object);
        }


        [Fact]
        public async Task GetTodoItems_ReturnsListOfTodoItems()
        {
            // Arrange
            ClearContext();

            var todoItems = new List<TodoItem> { 
                new TodoItem { Id = Guid.NewGuid(), Description = "Test 1" },
                new TodoItem { Id = Guid.NewGuid(), Description = "Test 2" }
            };
            _context.TodoItems.AddRange(todoItems);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetTodoItems();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); 
            Assert.Contains(result, item => item.Description == "Test 1"); 
            Assert.Contains(result, item => item.Description == "Test 2");
        }

        [Fact]
        public async Task GetTodoItem_ReturnsTodoItem()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todoItem = new TodoItem { Id = id, Description = "Test" };
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetTodoItem(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Test", result.Description);
        }

        [Fact]
        public async Task PutTodoItem_ReturnsSuccessWhenItemExists()
        {
            // Arrange
            ClearContext();
            var todoItem = new TodoItem { Id = Guid.NewGuid(), Description = "Test" };
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            todoItem.Description = "Updated Test";

            // Act
            var result = await _service.PutTodoItem(todoItem);

            // Assert
            Assert.True(result.Success);
            var updatedItem = await _context.TodoItems.FindAsync(todoItem.Id);
            Assert.NotNull(updatedItem);
            Assert.Equal("Updated Test", updatedItem.Description);
        }

        [Fact]
        public async Task PutTodoItem_ReturnsNotFoundWhenItemDoesNotExist()
        {
            // Arrange
            ClearContext();
            var todoItem = new TodoItem { Id = Guid.NewGuid(), Description = "Test" };

            // Act
            var result = await _service.PutTodoItem(todoItem);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Not Found", result.ErrorMessage);
        }

        [Fact]
        public async Task PostTodoItem_AddsNewItem()
        {
            // Arrange
            var todoItem = new TodoItem { Id = Guid.NewGuid(), Description = "Test" };

            // Act
            var result = await _service.PostTodoItem(todoItem);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(todoItem.Id, result.Id);
            Assert.Contains(todoItem, _context.TodoItems);
        }

        [Fact]
        public void TodoItemIdExists_ReturnsTrueWhenItemExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todoItem = new TodoItem { Id = id, Description = "Test" };
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();

            // Act
            var result = _service.TodoItemIdExists(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void TodoItemDescriptionExists_ReturnsTrueWhenDescriptionExists()
        {
            // Arrange
            var description = "Test";
            var todoItem = new TodoItem { Id = Guid.NewGuid(), Description = description };
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();

            // Act
            var result = _service.TodoItemDescriptionExists(description);

            // Assert
            Assert.True(result);
        }

        private void ClearContext()
        {
            _context.TodoItems.RemoveRange(_context.TodoItems);
            _context.SaveChanges();
        }
    }
}
