using ItemManagementSystem1.Data;
using ItemManagementSystem1.Models;
using ItemManagementSystem1.Repositories.ItemRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace ItemManagementSystem1.Tests
{
    public class ItemRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;

        public ItemRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetItemByIdAsync_ReturnsExpectedItem()
        {
            // Arrange
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var expectedItem = new Item
                {
                    Id = 1,
                    Name = "Test Item",
                    Description = "Test Description",
                    IsCompleted = false
                };
                context.Items.Add(expectedItem);
                await context.SaveChangesAsync();

                var repository = new ItemRepository(context);

                // Act
                var result = await repository.GetItemByIdAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(expectedItem.Id, result.Id);
                Assert.Equal(expectedItem.Name, result.Name);
                Assert.Equal(expectedItem.Description, result.Description);
                Assert.Equal(expectedItem.IsCompleted, result.IsCompleted);
            }
        }

        [Fact]
        public async Task GetItemByIdAsync_ReturnsNull_WhenItemNotFound()
        {
            // Arrange
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var repository = new ItemRepository(context);

                // Act
                var result = await repository.GetItemByIdAsync(1);

                // Assert
                Assert.Null(result);
            }
        }
        [Fact]
        public async Task AddItemAsync_ReturnExpectedItem()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                int itemId = 1;
                var expectedItem = new Item
                {
                    Id = 1,
                    Name = "Test",
                    Description = "Desctiption",
                    IsCompleted = false
                };


                var repository = new ItemRepository(context);
                repository.AddItemAsync(expectedItem);
                var actualItem = await repository.GetItemByIdAsync(itemId);

                Assert.NotNull(actualItem);
                Assert.Equal(expectedItem.Id, actualItem.Id);
                Assert.Equal(expectedItem.Description, actualItem.Description);
                Assert.Equal(expectedItem.Name, actualItem.Name);
                Assert.Equal(expectedItem.IsCompleted, actualItem.IsCompleted);
            }
        }

        [Fact]
        public async Task UpdateItemAsync_ReturnExpectedItem()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                int itemId = 1;
                var item = new Item
                {
                    Id = itemId,
                    Name = "Test",
                    Description = "Desctiption",
                    IsCompleted = false
                };

                var repository = new ItemRepository(context);
                await repository.AddItemAsync(item);
                var actualItem = await repository.GetItemByIdAsync(itemId);

                Assert.Equal(item.Id, actualItem.Id);
                Assert.Equal(item.Description, actualItem.Description);
                Assert.Equal(item.Name, actualItem.Name);
                Assert.Equal(item.IsCompleted, actualItem.IsCompleted);

                actualItem.Name = "Testss";
                repository.UpdateItem(actualItem);
                var updatedItem = await repository.GetItemByIdAsync(itemId);


                Assert.Equal(actualItem.Id, updatedItem.Id);
                Assert.Equal(actualItem.Description, updatedItem.Description);
                Assert.Equal(actualItem.Name, updatedItem.Name);
                Assert.Equal(actualItem.IsCompleted, updatedItem.IsCompleted);
            }
        }

        [Fact]
        public async Task Delete_ReturnExpectedItem()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                int itemId = 1;
                var item = new Item
                {
                    Id = itemId,
                    Name = "Test",
                    Description = "Desctiption",
                    IsCompleted = false
                };

                var repository = new ItemRepository(context);
                await repository.AddItemAsync(item);
                var actualItem = await repository.GetItemByIdAsync(itemId);

                Assert.Equal(item.Id, actualItem.Id);
                Assert.Equal(item.Description, actualItem.Description);
                Assert.Equal(item.Name, actualItem.Name);
                Assert.Equal(item.IsCompleted, actualItem.IsCompleted);

                actualItem.Name = "Test";
                repository.RemoveItem(actualItem);
                var deletedItem = await repository.GetItemByIdAsync(itemId);

                Assert.Null(deletedItem);
            }
        }
    }
}
