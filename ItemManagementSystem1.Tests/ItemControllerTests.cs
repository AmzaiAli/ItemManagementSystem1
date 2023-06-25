    using AutoMapper;
    using ItemManagementSystem1.Controllers;
    using ItemManagementSystem1.DTOs;
    using ItemManagementSystem1.Models;
    using ItemManagementSystem1.Services.ItemService;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System.Threading.Tasks;
    using Xunit;

    namespace ItemManagementSystem1.Tests
    {
    public class ItemControllerTests
    {
        private readonly Mock<IItemService> _mockItemService;
        private readonly Mock<IMapper> _mapper;

        public ItemControllerTests()
        {
            _mockItemService = new Mock<IItemService>();
            _mapper = new Mock<IMapper>();
            _mapper.Setup(m => m.Map<ItemDTO>(It.IsAny<ItemDTO>()))
                .Returns((ItemDTO source) => new ItemDTO
                {
                    Id = source.Id,
                    Name = source.Name,
                    Description = source.Description,
                    IsCompleted = source.IsCompleted
                });

        }

        [Fact]
        public async Task GetItemByIdAsync_ReturnsExpectedItem()
        {
            // Arrange
            var mockItemService = new Mock<IItemService>();
            var expectedItem = new ItemDTO { Id = 1, Name = "Test Item", Description = "Test Description",
                 IsCompleted = false };
            mockItemService.Setup(x => x.GetItemByIdAsync(1))
                           .ReturnsAsync(expectedItem);

            var controller = new ItemController(mockItemService.Object, _mapper.Object);

            // Act
            var result = await controller.GetItemByIdAsync(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualItem = Assert.IsType<ItemDTO>(okResult.Value);
            Assert.Equal(expectedItem.Id, actualItem.Id);
            Assert.Equal(expectedItem.Name, actualItem.Name);
            Assert.Equal(expectedItem.Description, actualItem.Description);
            Assert.Equal(expectedItem.IsCompleted, actualItem.IsCompleted);
        }


        [Fact]
        public async Task GetItemByIdAsync_ReturnsNotFoundResult_WhenItemIsNotFound()
        {
            // Arrange
            int nonExistentItemId = 100;
            ItemDTO nullItem = null;
            _mockItemService.Setup(repo => repo.GetItemByIdAsync(nonExistentItemId))
                .ReturnsAsync(nullItem);
            var controller = new ItemController(_mockItemService.Object, _mapper.Object);

            // Act
            var result = await controller.GetItemByIdAsync(nonExistentItemId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task CreateNewItem_ReturnsExpectedItem()
        {
            var mockItemService = new Mock<IItemService>();
            var expectedItem = new ItemDTO
            {
                Id = 1,
                Name = "Test Item",
                Description = "Desctiption",
                IsCompleted = false
            };
            mockItemService.Setup(x => x.CreateItemAsync(expectedItem))
                .ReturnsAsync(expectedItem);
            var controller = new ItemController(mockItemService.Object, _mapper.Object);

            var result = await controller.CreateItemAsync(expectedItem);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualItem = Assert.IsType<ItemDTO>(okResult.Value);
            Assert.Equal(expectedItem.Id, actualItem.Id);
            Assert.Equal(expectedItem.Description, actualItem.Description);
            Assert.Equal(expectedItem.Name, actualItem.Name);
            Assert.Equal(expectedItem.IsCompleted, actualItem.IsCompleted);
        }


        [Fact]
        public async Task Update_ReturnsNoContentFound()
        {
            var mockItemService = new Mock<IItemService>();
            var expectedItem = new ItemDTO
            {
                Id = 1,
                Name = "Test Item",
                Description = "Desctiption",
                IsCompleted = false
            };
            mockItemService.Setup(x => x.UpdateItemAsync(1, expectedItem));
            var controller = new ItemController(mockItemService.Object, _mapper.Object);

            var result = await controller.UpdateItemAsync(1, expectedItem);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest()
        {
            var mockItemService = new Mock<IItemService>();
            var expectedItem = new ItemDTO
            {
                Id = 1,
                Name = "Test Item",
                Description = "Desctiption",
                IsCompleted = false
            };
            mockItemService.Setup(x => x.UpdateItemAsync(1, expectedItem));
            var controller = new ItemController(mockItemService.Object, _mapper.Object);

            var result = await controller.UpdateItemAsync(10, expectedItem);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest()
        {
            var mockItemService = new Mock<IItemService>();
            mockItemService.Setup(x => x.DeleteItemAsync(1000000000));
            var controller = new ItemController(mockItemService.Object, _mapper.Object);

            var result = await controller.DeleteItemAsync(1000000000);

            Assert.IsType<NotFoundResult>(result);
        }
    }

    }
