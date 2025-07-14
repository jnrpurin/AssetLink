using AutoMapper;
using InventoryTracker.Dtos;
using InventoryTracker.Models;

namespace InventoryTracker.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
        }
    }
}
