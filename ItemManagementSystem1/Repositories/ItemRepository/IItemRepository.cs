using ItemManagementSystem1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItemManagementSystem1.Repositories.ItemRepository
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<Item> GetItemByIdAsync(int id);
        Task AddItemAsync(Item item);
        void UpdateItem(Item item);
        void RemoveItem(Item item);
    }
}
