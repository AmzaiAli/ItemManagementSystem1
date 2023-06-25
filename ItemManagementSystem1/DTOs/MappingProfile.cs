using AutoMapper;
using ItemManagementSystem1.Models;

namespace ItemManagementSystem1.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Item, ItemDTO>();
            CreateMap<ItemDTO, Item>();

        }
    }

}
