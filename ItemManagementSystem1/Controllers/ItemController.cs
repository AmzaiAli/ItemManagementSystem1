using AutoMapper;
using ItemManagementSystem1.DTOs;
using ItemManagementSystem1.Models;
using ItemManagementSystem1.Services;
using ItemManagementSystem1.Services.ItemService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItemManagementSystem1.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;

        public ItemController(IItemService itemService, IMapper mapper)
        {
            _itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
     //   [Authorize]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetAllItemsAsync()
        {
            var items = await _itemService.GetAllItemsAsync();
            var itemDTOs = _mapper.Map<IEnumerable<ItemDTO>>(items);
            return Ok(itemDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetItemByIdAsync(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            var itemDTO = _mapper.Map<ItemDTO>(item);
            return Ok(itemDTO);
        }

        [HttpPost]

        public async Task<ActionResult<ItemDTO>> CreateItemAsync(ItemDTO itemDTO)
        {
            await _itemService.CreateItemAsync(itemDTO);
            var newItemDTO = _mapper.Map<ItemDTO>(itemDTO);
            return Ok(newItemDTO);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(int id, ItemDTO itemDTO)
        {
            if (id != itemDTO.Id)
            {
                return BadRequest();
            }
            var item = _mapper.Map<Item>(itemDTO);
            try
            {
                await _itemService.UpdateItemAsync(id, itemDTO);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            await _itemService.DeleteItemAsync(item.Id);
            return NoContent();
        }
    }
}
