using CarpoolService.Contracts;
using CarPoolService.Models;
using CarPoolService.Models.DBModels;
using CarPoolService.Models.Interfaces.Repository_Interfaces;
using CarPoolService.Models.Interfaces.Service_Interface;

namespace CarpoolService.BAL.Services
{
    public class CarpoolRideService : ICarpoolService
    {
        private readonly ICarpoolRepository _rideRepository;

        public CarpoolRideService(ICarpoolRepository rideRepository)
        {
            _rideRepository = rideRepository;
        }

        public async Task<CarPoolRideDTO> OfferRide(CarPoolRide poolRide)
        {

            var offeredRide = await _rideRepository.OfferRide(poolRide);

            return offeredRide;
        }

        public async Task<IEnumerable<CarPoolRideDTO>> GetBookedRides(int userId)
        {
            var offeredRides = await _rideRepository.GetBookedRidesForUser(userId);

            return offeredRides;
        }

        public async Task<BookingDTO> BookRide(Booking bookRide)
        {
            var bookedRide = await _rideRepository.BookRide(bookRide);
            return bookedRide;
        }

        public async Task<IEnumerable<BookingDTO>> GetOfferedRides(int userId)
        {
            var bookedRides = await _rideRepository.GetOfferedRidesForUser(userId);
            return bookedRides;
        }

        public async Task<IEnumerable<CarPoolRideDTO>> MatchRides(Ride ride)
        {
            var matchRides = await _rideRepository.MatchRides(ride);
            return matchRides;
        }

        public async Task<IEnumerable<CityDTO>> GetCities()
        {
            var cities = await _rideRepository.GetCities();
            return cities;
        }
    }
}