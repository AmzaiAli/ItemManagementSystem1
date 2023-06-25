using AutoMapper;
using ItemManagementSystem1.DTOs;
using ItemManagementSystem1.Models;
using ItemManagementSystem1.Repositories;
using ItemManagementSystem1.Services.ItemService;
using Moq;


namespace ItemManagementSystem1.Tests
{
    public class ItemServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;


        public ItemServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

        }


        [Fact]
        public async Task GetItemByIdAsync_ReturnsExpectedItem()
        {
            // Arrange
            int itemId = 1;
            var item = new Item
            {
                Id = itemId,
                Name = "Test Item",
                Description = "Test Description",
                IsCompleted = false
            };
            var itemDto = new ItemDTO
            {
                Id = itemId,
                Name = "Test Item",
                Description = "Test Description",
                IsCompleted = false
            };
            _mockUnitOfWork.Setup(repo => repo.ItemRepository.GetItemByIdAsync(itemId)).ReturnsAsync(item);
            _mockMapper.Setup(m => m.Map<ItemDTO>(item)).Returns(itemDto);
            var itemService = new ItemService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act
            var result = await itemService.GetItemByIdAsync(itemId);

            // Assert
            Assert.Equal(itemDto, result);
        }

        [Fact]
        public async Task GetItemByIdAsync_ReturnsNull_WhenItemIsNotFound()
        {
            // Arrange
            int nonExistentItemId = 100;
            Item nullItem = null;
            _mockUnitOfWork.Setup(repo => repo.ItemRepository.GetItemByIdAsync(nonExistentItemId))
                .ReturnsAsync(nullItem);
            var itemService = new ItemService(_mockUnitOfWork.Object, _mockMapper.Object);

            // Act
            var result = await itemService.GetItemByIdAsync(nonExistentItemId);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task CreateNewItemAsync_ReturnExpectedItem()
        {
            int itemId = 1;
            var item = new Item
            {
                Id = itemId,
                Name = "Test",
                Description = "Description",
                IsCompleted = false
            };

            var itemDto = new ItemDTO
            {
                Id = itemId,
                Name = "Test",
                Description = "Description",
                IsCompleted = false
            };

            _mockUnitOfWork.Setup(rep => rep.ItemRepository.AddItemAsync(item));
            _mockMapper.Setup(m => m.Map<Item>(itemDto)).Returns(item);
            _mockMapper.Setup(m => m.Map<ItemDTO>(item)).Returns(itemDto);
            var itemService = new ItemService(_mockUnitOfWork.Object, _mockMapper.Object);
            var result = await itemService.CreateItemAsync(itemDto);
            Assert.Equal(itemDto, result);
        }

        [Fact]
        public async Task UpdateNewItemAsync_ReturnExpectedItem()
        {
            int itemId = 1;
            var item = new Item
            {
                Id = itemId,
                Name = "Test",
                Description = "Description",
                IsCompleted = false
            };

            Item item1 = null;

            var itemDto = new ItemDTO
            {
                Id = itemId,
                Name = "Test",
                Description = "Description",
                IsCompleted = false
            };
            _mockUnitOfWork.Setup(rep => rep.ItemRepository.GetItemByIdAsync(itemId))!.ReturnsAsync(item1);
            _mockUnitOfWork.Setup(rep => rep.ItemRepository.UpdateItem(item));
            _mockMapper.Setup(m => m.Map<Item>(itemDto)).Returns(item);
            _mockMapper.Setup(m => m.Map<ItemDTO>(item)).Returns(itemDto);
            var itemService = new ItemService(_mockUnitOfWork.Object, _mockMapper.Object);

            var ex = Assert.ThrowsAnyAsync<ArgumentException>(() => itemService.UpdateItemAsync(itemId, itemDto));
            Assert.Equal("Item not found.", ex.Result.Message);
        }


        [Fact]
        public async Task DeleteItemAsync_ReturnExpectedItem()
        {
            int itemId = 1;
            var item = new Item
            {
                Id = itemId,
                Name = "Test",
                Description = "Description",
                IsCompleted = false
            };

            Item item1 = null;
            _mockUnitOfWork.Setup(rep => rep.ItemRepository.GetItemByIdAsync(itemId))!.ReturnsAsync(item1);
            _mockUnitOfWork.Setup(rep => rep.ItemRepository.RemoveItem(item));
            var itemService = new ItemService(_mockUnitOfWork.Object, _mockMapper.Object);

            var ex = Assert.ThrowsAnyAsync<ArgumentException>(() => itemService.DeleteItemAsync(itemId));
            Assert.Equal("Item not found.", ex.Result.Message);
        }

    }
}
