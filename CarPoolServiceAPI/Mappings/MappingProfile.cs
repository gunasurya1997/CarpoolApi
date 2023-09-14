using AutoMapper;
using CarpoolService.Contracts;
using CarPoolService.Models.DBModels;

namespace CarPoolServiceAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap(); 
            CreateMap<CarPoolRide, CarPoolRideDTO>().ReverseMap();
            CreateMap<Booking,BookingDTO>().ReverseMap();
            CreateMap<City, CityDTO>().ReverseMap();
        }
    }
}
