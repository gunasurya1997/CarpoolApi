using CarpoolService.Contracts;
using CarPoolService.Models.DBModels;

namespace CarPoolService.Models.Interfaces.Service_Interface
{
    public interface ICarpoolService
    {
        Task<CarPoolRideDTO> OfferRide(CarPoolRide poolRide);
        Task<IEnumerable<BookingDTO>> GetOfferedRides(int userId);
        Task<BookingDTO> BookRide(Booking bookRide);
        Task<IEnumerable<CarPoolRideDTO>> GetBookedRides(int userId);
        Task<IEnumerable<CarPoolRideDTO>> MatchRides(Ride ride);
        Task<IEnumerable<CityDTO>> GetCities();
    }
}
