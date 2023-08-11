using CarpoolService.Contracts;
using CarPoolService.Models.DBModels;

namespace CarPoolService.Models.Interfaces.Repository_Interfaces
{
    public interface ICarpoolRepository
    {
        Task<CarPoolRideDTO> OfferRide(CarPoolRide poolRide);
        Task<List<CarPoolRideDTO>> GetOfferedRidesForUser(int userId);
        Task<BookingDTO> BookRide(Booking bookRide);
        Task<List<BookingDTO>> GetBookedRidesForUser(int userId);

        Task <List<CarPoolRideDTO>> MatchRides(Ride ride);
    }
}
