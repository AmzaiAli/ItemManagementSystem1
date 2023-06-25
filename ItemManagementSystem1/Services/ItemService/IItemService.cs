using ItemManagementSystem1.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItemManagementSystem1.Services.ItemService
{
    public interface IItemService
    {
        Task<IEnumerable<ItemDTO>> GetAllItemsAsync();
        Task<ItemDTO> GetItemByIdAsync(int id);
        Task<ItemDTO> CreateItemAsync(ItemDTO itemDto);
        Task UpdateItemAsync(int id, ItemDTO itemDto);
        Task DeleteItemAsync(int id);
    }
}
