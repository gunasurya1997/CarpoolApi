using CarpoolService.Contracts;
using CarPoolService.Models.DBModels;

namespace CarPoolService.Models.Interfaces.Repository_Interfaces
{
    public interface ICarpoolRepository
    {
        Task<CarPoolRideDTO> OfferRide(CarPoolRide poolRide);
        Task<IEnumerable<CarPoolRideDTO>> GetBookedRidesForUser(int userId);
        Task<BookingDTO> BookRide(Booking bookRide);
        Task<IEnumerable<BookingDTO>> GetOfferedRidesForUser(int userId);
        Task <IEnumerable<CarPoolRideDTO>> MatchRides(Ride ride);
        Task<IEnumerable<CityDTO>> GetCities();
    }
}
