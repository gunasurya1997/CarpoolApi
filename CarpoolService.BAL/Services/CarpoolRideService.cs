using AutoMapper;
using CarpoolService.Contracts; // Assuming necessary DTO namespaces
using CarPoolService.Models;
using CarPoolService.Models.DBModels;
using CarPoolService.Models.Interfaces.Repository_Interfaces;
using CarPoolService.Models.Interfaces.Service_Interface;

namespace CarpoolService.BAL.Services
{
    public class CarpoolRideService : ICarpoolService
    {
        private readonly ICarpoolRepository _rideRepository;
        private readonly IMapper _mapper;

        public CarpoolRideService(ICarpoolRepository rideRepository, IMapper mapper)
        {
            _rideRepository = rideRepository;
            _mapper = mapper;
        }

        public async Task<CarPoolRideDTO> OfferRide(CarPoolRide poolRide)
        {

            var offeredRide = await _rideRepository.OfferRide(poolRide);

            return offeredRide;
        }

        public async Task<List<CarPoolRideDTO>> GetBookedRides(int userId)
        {
            var offeredRides = await _rideRepository.GetBookedRidesForUser(userId);

            return offeredRides;
        }

        public async Task<BookingDTO> BookRide(Booking bookRide)
        {
            var bookedRide = await _rideRepository.BookRide(bookRide);
            return bookedRide;
        }

        public async Task<List<BookingDTO>> GetOfferedRides(int userId)
        {
            var bookedRides = await _rideRepository.GetOfferedRidesForUser(userId);
            return bookedRides;
        }

        public async Task<List<CarPoolRideDTO>> MatchRides(Ride ride)
        {
            var matchRides = await _rideRepository.MatchRides(ride);
            return matchRides;
        }

        public async Task<List<CityDTO>> GetCities()
        {
            var cities = await _rideRepository.GetCities();
            return cities;
        }
    }
}