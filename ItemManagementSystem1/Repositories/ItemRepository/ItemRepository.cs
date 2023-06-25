using ItemManagementSystem1.Data;
using ItemManagementSystem1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItemManagementSystem1.Repositories.ItemRepository
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _dbContext;

        public ItemRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await _dbContext.Items.ToListAsync();
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            return await _dbContext.Items.FindAsync(id);
        }

        public async Task AddItemAsync(Item item)
        {
            await _dbContext.Items.AddAsync(item);
        }

        public void UpdateItem(Item item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }

        public void RemoveItem(Item item)
        {
            _dbContext.Items.Remove(item);
        }
    }
}
