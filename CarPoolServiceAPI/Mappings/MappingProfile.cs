using AutoMapper;
using CarpoolService.Contracts;
using CarPoolService.Models.DBModels;

namespace CarPoolServiceAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>(); 
            CreateMap<UserDto, User>();
            CreateMap<CarPoolRideDTO, CarPoolRide>();
            CreateMap<CarPoolRide, CarPoolRideDTO>();
            CreateMap<Booking,BookingDTO>();
            CreateMap<BookingDTO,Booking>();
            CreateMap<CityDTO, City>();
            CreateMap<City, CityDTO>();
        }
    }
}
