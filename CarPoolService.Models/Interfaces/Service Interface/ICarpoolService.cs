using CarpoolService.Contracts;
using CarPoolService.Models.DBModels;

namespace CarPoolService.Models.Interfaces.Service_Interface
{
    public interface ICarpoolService
    {
        Task<CarPoolRideDTO> OfferRide(CarPoolRide poolRide);
        Task<List<BookingDTO>> GetOfferedRides(int userId);
        Task<BookingDTO> BookRide(Booking bookRide);
        Task<List<CarPoolRideDTO>> GetBookedRides(int userId);
        Task<List<CarPoolRideDTO>> MatchRides(Ride ride);
        Task<List<CityDTO>> GetCities();
    }
}
