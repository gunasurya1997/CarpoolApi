using CarpoolService.Contracts;
using CarPoolService.Models.DBModels;

namespace CarPoolService.Contracts.Interfaces.Repository_Interfaces
{
    public interface ICarpoolRepository
    {
        Task<CarPoolRideDTO> CreateOfferRide(CarPoolRide poolRide);
        Task<IEnumerable<CarPoolRideDTO>> GetAllOfferedRidesForUser(int userId);
        Task<BookingDTO> CreateBookRide(Booking bookRide);
        Task<IEnumerable<BookingDTO>> GetAllBookedRidesForUser(int userId);
        Task<IEnumerable<CityDTO>> GetCities();
    }
}
