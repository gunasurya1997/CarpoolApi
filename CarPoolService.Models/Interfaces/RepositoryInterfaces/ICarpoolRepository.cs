using CarpoolService.Contracts;
using CarPoolService.Models.DBModels;

namespace CarPoolService.Models.Interfaces.Repository_Interfaces
{
    public interface ICarpoolRepository
    {
        Task<CarPoolRideDTO> OfferRide(CarPoolRide poolRide);
        Task<List<CarPoolRideDTO>> GetBookedRidesForUser(int userId);
        Task<BookingDTO> BookRide(Booking bookRide);
        Task<List<BookingDTO>> GetOfferedRidesForUser(int userId);
        Task <List<CarPoolRideDTO>> MatchRides(Ride ride);
        Task<List<CityDTO>> GetCities();
    }
}
