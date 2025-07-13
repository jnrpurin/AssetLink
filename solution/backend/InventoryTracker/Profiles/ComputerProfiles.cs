using AutoMapper;
using InventoryTracker.Dtos;
using InventoryTracker.Models;

namespace InventoryTracker.Profiles
{
    public class ComputerProfile : Profile
    {
        public ComputerProfile()
        {
            CreateMap<Computer, ComputerReadDto>()
                .ForMember(dest => dest.ComputerManufacturerName,
                        opt => opt.MapFrom(src => src.ComputerManufacturer != null ? src.ComputerManufacturer.Name : string.Empty))
                .ForMember(dest => dest.AssignedTo,
                        opt => opt.MapFrom(src => src.ComputerUsers
                                        .Where(lcu => lcu.AssignEndDt == null)
                                        .OrderByDescending(lcu => lcu.AssignStartDt)
                                        .Select(lcu => lcu.User!.FirstName)
                                        .FirstOrDefault()))
                .ForMember(dest => dest.AssignedOnDt,
                        opt => opt.MapFrom(src => src.ComputerUsers
                                        .Where(lcu => lcu.AssignEndDt == null)
                                        .OrderByDescending(lcu => lcu.AssignStartDt)
                                        .Select(lcu => lcu.AssignStartDt)
                                        .FirstOrDefault()))
                .ForMember(dest => dest.Status,
                        opt => opt.MapFrom(src => src.ComputerStatuses
                                        .OrderByDescending(lccc => lccc.AssignDt) 
                                        .Select(lccc => lccc.ComputerStatus!.LocalizedName) 
                                        .FirstOrDefault())); 

            CreateMap<ComputerCreateDto, Computer>();
            CreateMap<ComputerUpdateDto, Computer>();
            
            CreateMap<ComputerAssignUserDto, LnkComputerUser>()
                .ForMember(dest => dest.AssignStartDt,
                           opt => opt.MapFrom(src => src.AssignStartDt ?? DateTime.UtcNow));
        }
    }
}
