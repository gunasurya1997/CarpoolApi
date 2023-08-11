using AutoMapper;
using CarpoolService.Contracts;
using CarPoolService.Models.DBModels;

namespace CarPoolServiceAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>(); // Map User to UserDto
            CreateMap<UserDto, User>(); // Reverse mapping
            CreateMap<CarPoolRideDTO, CarPoolRide>();
            CreateMap<CarPoolRide, CarPoolRideDTO>();
            CreateMap<Booking,BookingDTO>();
            CreateMap<BookingDTO,Booking>();
            // Add other mappings here if needed
        }
    }
}
