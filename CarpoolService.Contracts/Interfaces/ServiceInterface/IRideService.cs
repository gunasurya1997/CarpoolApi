using CarPoolService.Models;
using CarPoolService.Models.DBModels;
using DTO = CarpoolService.Contracts.DTOs;


namespace CarpoolService.Contracts.Interfaces.ServiceInterface
{
    public interface IRideService
    {
        Task<DTO.Ride> CreateOfferRide(CarPoolRide poolRide);
        Task<IEnumerable<DTO.Booking>> GetOfferedRides(int userId);
        Task<DTO.Booking> CreateBookRide(Booking bookRide);
        Task<IEnumerable<DTO.Ride>> GetBookedRides(int userId);
        Task<IEnumerable<DTO.Ride>> FindMatchRides(Ride ride);
        Task<IEnumerable<DTO.City>> GetCities();
    }
}
