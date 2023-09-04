using AutoMapper;
using CarpoolService.Contracts;
using CarPoolService.DAL;
using CarPoolService.Models;
using CarPoolService.Models.DBModels;
using CarPoolService.Models.Interfaces.Repository_Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarpoolService.DAL.Repositories
{
    public class CarPoolRideRepository : ICarpoolRepository
    {
        private readonly CarpoolDbContext _dbContext;
        private readonly IMapper _mapper;

        public CarPoolRideRepository(CarpoolDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;

        }

        // Method for offering a ride
        public async Task<CarPoolRideDTO> OfferRide(CarPoolRide poolRide)
        {
            await _dbContext.CarPoolRides.AddAsync(poolRide);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<CarPoolRideDTO>(poolRide);
        }

        // Method for getting booked rides for a user
        public async Task<IEnumerable<CarPoolRideDTO>> GetBookedRidesForUser(int userId)
        {
            try
            {
                IEnumerable<CarPoolRide> bookedRides = await (
                    from ride in _dbContext.CarPoolRides
                    where _dbContext.Bookings.Any(booking => booking.PassengerId == userId && booking.RideId == ride.RideId)
                    select ride).ToListAsync();

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
                    Fare = cpRide.Fare ?? string.Empty,
                    DriverName = drivers.FirstOrDefault(d => d.UserId == cpRide.DriverId)?.UserName,
                    DriverImage = drivers.FirstOrDefault(d => d.UserId == cpRide.DriverId)?.Image,
                }).ToList();

                return BookedRideDTOs;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving offered rides: " + e.Message);
            }
        }

        // Method for booking a ride
        public async Task<BookingDTO> BookRide(Booking bookRide)
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

                    await _dbContext.Bookings.AddAsync(bookRide);
                    await _dbContext.SaveChangesAsync();

                    _dbContext.CarPoolRides.Update(offeredRide);
                    await _dbContext.SaveChangesAsync();

                    return _mapper.Map<BookingDTO>(bookRide);
                }

                throw new InvalidOperationException("No matching ride available for booking.");
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while booking a ride: " + e.Message, e);
            }
        }

        // Method for getting offered rides for a user
        public async Task<IEnumerable<BookingDTO>> GetOfferedRidesForUser(int userId)
        {
            try
            {
                IEnumerable<Booking> offeredRides = await (
                from booking in _dbContext.Bookings
                where _dbContext.CarPoolRides.Any(ride => ride.RideId == booking.RideId && ride.DriverId == userId)
                select booking).ToListAsync();

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
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving booked rides: " + e.Message);
            }
        }

        // Method to get matching rides based on user's ride request
        public async Task<IEnumerable<CarPoolRideDTO>> MatchRides(Ride ride)
        {
            try
            {
                IEnumerable<CarPoolRide> matchedRides = await _dbContext.CarPoolRides
                    .Where(cpRide =>
                         cpRide.DepartureCityId == ride.StartPoint &&
                         cpRide.DestinationCityId == ride.EndPoint &&
                         cpRide.Date == ride.Date &&
                         cpRide.TimeSlot == ride.TimeSlot &&
                         cpRide.DriverId != ride.UserId &&
                         cpRide.RideStatus == false)
                    .ToListAsync();

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
            catch (Exception e)
            {
                throw new Exception("An error occurred while matching rides: " + e.Message, e);
            }
        }


        // Method for getting a list of cities
        public async Task<IEnumerable<CityDTO>> GetCities()
        {
            try
            {
                IEnumerable<City> cities = await _dbContext.Cities.ToListAsync();
                return _mapper.Map<IEnumerable<CityDTO>>(cities);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving cities: " + e.Message);
            }
        }

    }

}