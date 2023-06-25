using AutoMapper;
using ItemManagementSystem1.DTOs;
using ItemManagementSystem1.Models;
using ItemManagementSystem1.Repositories;

namespace ItemManagementSystem1.Services.ItemService
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemDTO>> GetAllItemsAsync()
        {
            var items = await _unitOfWork.ItemRepository.GetAllItemsAsync();
            return _mapper.Map<IEnumerable<ItemDTO>>(items);
        }

        public async Task<ItemDTO> GetItemByIdAsync(int id)
        {
            var item = await _unitOfWork.ItemRepository.GetItemByIdAsync(id);
            return _mapper.Map<ItemDTO>(item);
        }

        public async Task<ItemDTO> CreateItemAsync(ItemDTO itemDto)
        {
            try
            {
                var item = _mapper.Map<Item>(itemDto);
                _unitOfWork.ItemRepository.AddItemAsync(item);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<ItemDTO>(item);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task UpdateItemAsync(int id, ItemDTO itemDto)
        {
            try
            {
                var item = await _unitOfWork.ItemRepository.GetItemByIdAsync(id);
                if (item == null)
                {
                    throw new ArgumentException("Item not found.");
                }
                _mapper.Map(itemDto, item);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw ex;
            }
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = await _unitOfWork.ItemRepository.GetItemByIdAsync(id);
            if (item == null)
            {
                throw new ArgumentException("Item not found.");
            }
            _unitOfWork.ItemRepository.RemoveItem(item);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
