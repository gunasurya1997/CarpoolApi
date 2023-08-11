using CarpoolService.Contracts;
using CarPoolService.Models.DBModels;

namespace CarPoolService.Models.Interfaces.Service_Interface
{
    public interface ICarpoolService
    {
        Task<CarPoolRideDTO> OfferRide(CarPoolRide poolRide);
        Task<List<CarPoolRideDTO>> GetOfferedRides(int userId);
        Task<BookingDTO> BookRide(Booking bookRide);
        Task<List<BookingDTO>> GetBookedRides(int userId);
        Task<List<CarPoolRideDTO>> MatchRides(Ride ride);
    }
}
