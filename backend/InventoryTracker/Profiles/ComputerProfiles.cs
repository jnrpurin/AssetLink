using AutoMapper;
using InventoryTracker.Dtos;
using InventoryTracker.Models;

namespace InventoryTracker.Profiles
{
    public class ComputerProfile : Profile
    {
        public ComputerProfile()
        {
            CreateMap<Computer, ComputerReadDto>();
            CreateMap<ComputerCreateDto, Computer>();
            CreateMap<ComputerUpdateDto, Computer>();
        }
    }
}
