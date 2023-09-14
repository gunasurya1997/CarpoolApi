using CarpoolService.Common.Exceptions;
using CarpoolService.Contracts;
using CarPoolService.Contracts.Interfaces.Repository_Interfaces;
using CarPoolService.Contracts.Interfaces.Service_Interface;
using CarPoolService.DAL;
using CarPoolService.Models;
using CarPoolService.Models.DBModels;
using Microsoft.EntityFrameworkCore;

namespace CarpoolService.BAL.Services
{
    public class CarpoolRideService : ICarpoolService
    {
        private readonly CarpoolDbContext _dbContext;
        private readonly ICarpoolRepository _rideRepository;

        public CarpoolRideService(ICarpoolRepository rideRepository, CarpoolDbContext dbContext)
        {
            _rideRepository = rideRepository;
            _dbContext = dbContext;
        }

        #region OfferRide
        // Create an offer ride
        public async Task<CarPoolRideDTO> CreateOfferRide(CarPoolRide poolRide)
        {
            try
            {
                CarPoolRideDTO offeredRide = await _rideRepository.CreateOfferRide(poolRide);
                return offeredRide;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating an offer ride." + ex.Message, ex);
            }
        }

        // Get offered rides for a user
        public async Task<IEnumerable<BookingDTO>> GetOfferedRides(int userId)
        {
            try
            {
                IEnumerable<BookingDTO> allBookedRides = await _rideRepository.GetAllBookedRidesForUser(userId);
                IEnumerable<BookingDTO> offeredRides = allBookedRides.Where(bookedRide => _dbContext.CarPoolRides
                .Any(ride => ride.RideId == bookedRide.RideId && ride.DriverId == userId)).ToList();

                IEnumerable<int> cityIds = offeredRides.Select(bookedRide => bookedRide.PickupLocationId).Union(offeredRides.Select(bookedRide => bookedRide.DropLocationId)).Distinct().ToList();
                IEnumerable<City> cities = await _dbContext.Cities.Where(city => cityIds.Contains(city.CityId)).ToListAsync();
                IEnumerable<int> passengerIds = offeredRides.Select(bookedRide => bookedRide.PassengerId).ToList();
                IEnumerable<User> passengers = await _dbContext.Users.Where(user => passengerIds.Contains(user.UserId)).ToListAsync();
                IEnumerable<BookingDTO> OfferedRideDTOs = offeredRides.Select(bookedRide => new BookingDTO
                {
                    PickupLocation = cities.FirstOrDefault(c => c.CityId == bookedRide.PickupLocationId)?.CityName,
                    DropLocation = cities.FirstOrDefault(c => c.CityId == bookedRide.DropLocationId)?.CityName,
                    PickupLocationId = bookedRide.PickupLocationId,
                    DropLocationId = bookedRide.DropLocationId,
                    PassengerId = bookedRide.PassengerId,
                    RideId = bookedRide.RideId,
                    TimeSlot = bookedRide.TimeSlot ?? string.Empty,
                    Date = bookedRide.Date,
                    ReservedSeats = bookedRide.ReservedSeats,
                    Fare = bookedRide.Fare,
                    PassengerName = passengers.FirstOrDefault(d => d.UserId == bookedRide.PassengerId)?.UserName,
                    PassengerImage = passengers.FirstOrDefault(d => d.UserId == bookedRide.PassengerId)?.Image,
                }).ToList();
                return OfferedRideDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting offered rides for a user." + ex.Message, ex);
            }
        }
        #endregion

        #region BookRide
        // Create a booked ride

        public async Task<BookingDTO> CreateBookRide(Booking bookRide)
        {
            try
            {
                CarPoolRide? offeredRide = await _dbContext.CarPoolRides.FirstOrDefaultAsync(x =>
                    x.DepartureCityId == bookRide.PickupLocationId &&
                    x.DestinationCityId == bookRide.DropLocationId &&
                    x.TimeSlot == bookRide.TimeSlot &&
                    x.Date == bookRide.Date &&
                    x.AvailableSeats == bookRide.ReservedSeats
                );
                if (offeredRide != null)
                {
                    offeredRide.RideStatus = true;
                    _dbContext.CarPoolRides.Update(offeredRide);
                    await _dbContext.SaveChangesAsync();
                    BookingDTO bookedRide = await _rideRepository.CreateBookRide(bookRide);
                    return bookedRide;
                }
                throw new NotFoundException("The Match Ride was not Found");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating a booked ride." + ex.Message, ex);
            }
        }

        // Get booked rides for a user
        public async Task<IEnumerable<CarPoolRideDTO>> GetBookedRides(int userId)
        {
            try
            {
                IEnumerable<CarPoolRideDTO> allOfferedRides = await _rideRepository.GetAllOfferedRidesForUser(userId);
                IEnumerable<CarPoolRideDTO> bookedRides = allOfferedRides.Where(offeredRide => _dbContext.Bookings
              .Any(ride => ride.RideId == offeredRide.RideId && ride.PassengerId == userId)).ToList();

                IEnumerable<int> cityIds = bookedRides.Select(cpRide => cpRide.DepartureCityId).Union(bookedRides.Select(cpRide => cpRide.DestinationCityId)).Distinct().ToList();
                IEnumerable<City> cities = await _dbContext.Cities.Where(city => cityIds.Contains(city.CityId)).ToListAsync();
                IEnumerable<int> driverIds = bookedRides.Select(cpRide => cpRide.DriverId).ToList();
                IEnumerable<User> drivers = await _dbContext.Users.Where(user => driverIds.Contains(user.UserId)).ToListAsync();
                IEnumerable<CarPoolRideDTO> BookedRideDTOs = bookedRides.Select(cpRide => new CarPoolRideDTO
                {
                    DepartureCityName = cities.FirstOrDefault(c => c.CityId == cpRide.DepartureCityId)?.CityName,
                    DestinationCityName = cities.FirstOrDefault(c => c.CityId == cpRide.DestinationCityId)?.CityName,
                    DepartureCityId = cpRide.DepartureCityId,
                    DestinationCityId = cpRide.DestinationCityId,
                    DriverId = cpRide.DriverId,
                    RideId = cpRide.RideId,
                    Stops = cpRide.Stops,
                    TimeSlot = cpRide.TimeSlot,
                    Date = cpRide.Date,
                    AvailableSeats = cpRide.AvailableSeats,
                    RideStatus = cpRide.RideStatus,
                    Fare = cpRide.Fare,
                    DriverName = drivers.FirstOrDefault(d => d.UserId == cpRide.DriverId)?.UserName,
                    DriverImage = drivers.FirstOrDefault(d => d.UserId == cpRide.DriverId)?.Image,
                }).ToList();

                return BookedRideDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting booked rides for a user." + ex.Message, ex);
            }
        }
        #endregion

        // Find matching rides for a given ride
        public async Task<IEnumerable<CarPoolRideDTO>> FindMatchRides(Ride ride)
        {
            try
            {

                IEnumerable<CarPoolRideDTO> allOfferedRides = await _rideRepository.GetAllOfferedRidesForUser(ride.UserId);
                IEnumerable<CarPoolRideDTO> matchedRides = allOfferedRides.Where(cpRide =>
                             cpRide.DepartureCityId == ride.StartPoint &&
                             cpRide.DestinationCityId == ride.EndPoint &&
                             cpRide.Date == ride.Date &&
                             cpRide.TimeSlot == ride.TimeSlot &&
                             cpRide.DriverId != ride.UserId &&
                             cpRide.RideStatus == false).ToList();

                IEnumerable<int> cityIds = matchedRides.Select(cpRide => cpRide.DepartureCityId).Union(matchedRides.Select(cpRide => cpRide.DestinationCityId)).Distinct().ToList();
                IEnumerable<City> cities = await _dbContext.Cities.Where(city => cityIds.Contains(city.CityId)).ToListAsync();
                IEnumerable<int> driverIds = matchedRides.Select(cpRide => cpRide.DriverId).ToList();
                IEnumerable<User> drivers = await _dbContext.Users.Where(user => driverIds.Contains(user.UserId)).ToListAsync();
                IEnumerable<CarPoolRideDTO> matchedRideDTOs = matchedRides.Select(cpRide => new CarPoolRideDTO
                {
                    DepartureCityName = cities.FirstOrDefault(c => c.CityId == cpRide.DepartureCityId)?.CityName,
                    DestinationCityName = cities.FirstOrDefault(c => c.CityId == cpRide.DestinationCityId)?.CityName,
                    DepartureCityId = cpRide.DepartureCityId,
                    DestinationCityId = cpRide.DestinationCityId,
                    DriverId = cpRide.DriverId,
                    RideId = cpRide.RideId,
                    Stops = cpRide.Stops,
                    TimeSlot = cpRide.TimeSlot,
                    Date = cpRide.Date,
                    AvailableSeats = cpRide.AvailableSeats,
                    RideStatus = cpRide.RideStatus,
                    Fare = cpRide.Fare,
                    DriverName = drivers.FirstOrDefault(d => d.UserId == cpRide.DriverId)?.UserName,
                    DriverImage = drivers.FirstOrDefault(d => d.UserId == cpRide.DriverId)?.Image,
                }).ToList();
                return matchedRideDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting match rides for a user." + ex.Message, ex);
            }
        }

        // Get a list of cities 
        public async Task<IEnumerable<CityDTO>> GetCities()
        {
            IEnumerable<CityDTO> cities = await _rideRepository.GetCities();
            return cities;
        }
    }
}