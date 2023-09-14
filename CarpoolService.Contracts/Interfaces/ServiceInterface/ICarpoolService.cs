using CarpoolService.Contracts;
using CarPoolService.Models;
using CarPoolService.Models.DBModels;

namespace CarPoolService.Contracts.Interfaces.Service_Interface
{
    public interface ICarpoolService
    {
        Task<CarPoolRideDTO> CreateOfferRide(CarPoolRide poolRide);
        Task<IEnumerable<BookingDTO>> GetOfferedRides(int userId);
        Task<BookingDTO> CreateBookRide(Booking bookRide);
        Task<IEnumerable<CarPoolRideDTO>> GetBookedRides(int userId);
        Task<IEnumerable<CarPoolRideDTO>> FindMatchRides(Ride ride);
        Task<IEnumerable<CityDTO>> GetCities();
    }
}
