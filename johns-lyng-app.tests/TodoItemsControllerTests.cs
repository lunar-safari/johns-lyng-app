using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using johns_lyng_app.Server.Controllers;
using johns_lyng_app.Server.Interface;
using johns_lyng_app.Server.Models;

namespace TodoList.Api.UnitTests
{
    public class TodoItemsControllerTests
    {
        private readonly Mock<ILogger<TodoItemsController>> _mockLogger;
        private readonly Mock<ITodoItemsService> _mockTodoItemsService;
        private readonly TodoItemsController _controller;

        public TodoItemsControllerTests()
        {
            _mockLogger = new Mock<ILogger<TodoItemsController>>();
            _mockTodoItemsService = new Mock<ITodoItemsService>();
            _controller = new TodoItemsController(_mockLogger.Object, _mockTodoItemsService.Object);
        }
              
        [Fact]
        public async Task GetTodoItems_ReturnsOkResult_WithListOfTodoItems()
        {
            // Arrange
            var mockItems = new List<TodoItem> { new TodoItem { Id = Guid.NewGuid(), Description = "Test Item", IsCompleted = false } };
            _mockTodoItemsService.Setup(service => service.GetTodoItems()).ReturnsAsync(mockItems);

            // Act
            var result = await _controller.GetTodoItems();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnItems = Assert.IsType<List<TodoItem>>(okResult.Value);
            Assert.Equal(mockItems.Count, returnItems.Count);
        }

        [Fact]
        public async Task GetTodoItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            _mockTodoItemsService.Setup(service => service.GetTodoItem(It.IsAny<Guid>())).ReturnsAsync((TodoItem)null);

            // Act
            var result = await _controller.GetTodoItem(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetTodoItem_ReturnsOkResult_WithTodoItem()
        {
            // Arrange
            var mockItem = new TodoItem { Id = Guid.NewGuid(), Description = "Test Item", IsCompleted = false };
            _mockTodoItemsService.Setup(service => service.GetTodoItem(It.IsAny<Guid>())).ReturnsAsync(mockItem);

            // Act
            var result = await _controller.GetTodoItem(mockItem.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnItem = Assert.IsType<TodoItem>(okResult.Value);
            Assert.Equal(mockItem.Id, returnItem.Id);
        }

        [Fact]
        public async Task PutTodoItem_ReturnsBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            var mockItem = new TodoItem { Id = Guid.NewGuid(), Description = "Test Item", IsCompleted = false };

            // Act
            var result = await _controller.PutTodoItem(Guid.NewGuid(), mockItem);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutTodoItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            var mockItem = new TodoItem { Id = Guid.NewGuid(), Description = "Test Item", IsCompleted = false };
            _mockTodoItemsService.Setup(service => service.PutTodoItem(It.IsAny<TodoItem>()))
                .ReturnsAsync(new OperationResult { Success = false, ErrorMessage = "Not Found" });

            // Act
            var result = await _controller.PutTodoItem(mockItem.Id, mockItem);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutTodoItem_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var mockItem = new TodoItem { Id = Guid.NewGuid(), Description = "Test Item", IsCompleted = false };
            _mockTodoItemsService.Setup(service => service.PutTodoItem(It.IsAny<TodoItem>()))
                .ReturnsAsync(new OperationResult { Success = true });

            // Act
            var result = await _controller.PutTodoItem(mockItem.Id, mockItem);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PostTodoItem_ReturnsBadRequest_WhenDescriptionIsMissing()
        {
            // Arrange
            var mockItem = new TodoItem { Id = Guid.NewGuid(), Description = "", IsCompleted = false };

            // Act
            var result = await _controller.PostTodoItem(mockItem);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Description is required", badRequestResult.Value);
        }

        [Fact]
        public async Task PostTodoItem_ReturnsBadRequest_WhenDescriptionExists()
        {
            // Arrange
            var mockItem = new TodoItem { Id = Guid.NewGuid(), Description = "Test Item", IsCompleted = false };
            _mockTodoItemsService.Setup(service => service.TodoItemDescriptionExists(It.IsAny<string>())).Returns(true);

            // Act
            var result = await _controller.PostTodoItem(mockItem);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Description already exists", badRequestResult.Value);
        }

        [Fact]
        public async Task PostTodoItem_ReturnsCreatedAtAction_WhenSuccessful()
        {
            // Arrange
            var mockItem = new TodoItem { Id = Guid.NewGuid(), Description = "Test Item", IsCompleted = false };
            _mockTodoItemsService.Setup(service => service.PostTodoItem(It.IsAny<TodoItem>())).ReturnsAsync(mockItem);

            // Act
            var result = await _controller.PostTodoItem(mockItem);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetTodoItem), createdAtActionResult.ActionName);
            Assert.Equal(mockItem.Id, ((TodoItem)createdAtActionResult.Value).Id);
        }
              
    }
}
