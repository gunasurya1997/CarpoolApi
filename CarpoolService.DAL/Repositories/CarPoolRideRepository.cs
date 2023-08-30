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
        public async Task<CarPoolRideDTO> OfferRide(CarPoolRide poolRide)
        {
            await _dbContext.CarPoolRides.AddAsync(poolRide);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<CarPoolRideDTO>(poolRide);

        }


        public async Task<List<CarPoolRideDTO>> GetBookedRidesForUser(int userId)
        {
            try
            {
                List<CarPoolRide> offeredRides = await _dbContext.CarPoolRides
                    .Where(ride => _dbContext.Bookings
                        .Any(booking => booking.PassengerId == userId && booking.RideId == ride.RideId))
                    .ToListAsync();
                var cityIds = offeredRides.Select(cpRide => cpRide.DepartureCityId).Union(offeredRides.Select(cpRide => cpRide.DestinationCityId)).Distinct().ToList();
                var cities = await _dbContext.Cities.Where(city => cityIds.Contains(city.CityId)).ToListAsync();
                var driverIds = offeredRides.Select(cpRide => cpRide.DriverId).ToList();
                var drivers = await _dbContext.Users.Where(user => driverIds.Contains(user.UserId)).ToListAsync();
                var matchedRideDTOs = offeredRides.Select(cpRide => new CarPoolRideDTO
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
                    // Make sure you map other properties of CarPoolRideDTO using cpRide here
                }).ToList();

                return matchedRideDTOs;

            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving offered rides: " + e.Message);
            }
        }


        public async Task<BookingDTO> BookRide(Booking bookRide)
        {
            try
            {
                var offeredRide = await _dbContext.CarPoolRides.FirstOrDefaultAsync(x =>
                        x.DepartureCityId == bookRide.PickupLocationId &&
                        x.DestinationCityId == bookRide.DropLocationId &&
                        x.TimeSlot == bookRide.TimeSlot &&
                        x.Date == bookRide.Date &&
                        x.AvailableSeats == bookRide.ReservedSeats
                    );

                if (offeredRide != null)
                {
                    // Update the offered ride details
                    offeredRide.RideStatus = true;

                    await _dbContext.Bookings.AddAsync(bookRide);
                    await _dbContext.SaveChangesAsync();

                    // Update the offered ride in the database
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


        public async Task<List<BookingDTO>> GetOfferedRidesForUser(int userId)
        {
            try
            {
                List<Booking> bookedRides = await _dbContext.Bookings
                    .Where(booking => _dbContext.CarPoolRides
                        .Any(ride => ride.RideId == booking.RideId && ride.DriverId == userId))
                    .ToListAsync();
                var cityIds = bookedRides.Select(bookedRide => bookedRide.PickupLocationId).Union(bookedRides.Select(bookedRide => bookedRide.DropLocationId)).Distinct().ToList();
                var cities = await _dbContext.Cities.Where(city => cityIds.Contains(city.CityId)).ToListAsync();
                var passengerIds = bookedRides.Select(bookedRide => bookedRide.PassengerId).ToList();
                var passengers = await _dbContext.Users.Where(user => passengerIds.Contains(user.UserId)).ToListAsync();
                var matchedRideDTOs = bookedRides.Select(bookedRide => new BookingDTO
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
                    // Make sure you map other properties of CarPoolRideDTO using cpRide here
                }).ToList();

                return matchedRideDTOs;

            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving booked rides: " + e.Message);
            }
        }


        public async Task<List<CarPoolRideDTO>> MatchRides(Ride ride)
        {
            try
            {
                var matchedRides = await _dbContext.CarPoolRides
                    .Where(cpRide =>
                         cpRide.DepartureCityId == ride.StartPoint &&
                         cpRide.DestinationCityId == ride.EndPoint &&
                         cpRide.Date == ride.Date &&
                         cpRide.TimeSlot == ride.TimeSlot &&
                         cpRide.RideStatus == false)
                    .ToListAsync();

                var cityIds = matchedRides.Select(cpRide => cpRide.DepartureCityId).Union(matchedRides.Select(cpRide => cpRide.DestinationCityId)).Distinct().ToList();
                var cities = await _dbContext.Cities.Where(city => cityIds.Contains(city.CityId)).ToListAsync();
                var driverIds = matchedRides.Select(cpRide => cpRide.DriverId).ToList();
                var drivers = await _dbContext.Users.Where(user => driverIds.Contains(user.UserId)).ToListAsync();
                var matchedRideDTOs = matchedRides.Select(cpRide => new CarPoolRideDTO
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
                    // Make sure you map other properties of CarPoolRideDTO using cpRide here
                }).ToList();

                return matchedRideDTOs;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while matching rides: " + e.Message, e);
            }
        }



        public async Task<List<CityDTO>> GetCities()
        {
            try
            {
                IEnumerable<City> cities = await _dbContext.Cities.ToListAsync();
                return _mapper.Map<List<CityDTO>>(cities);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving cities: " + e.Message);
            }
        }

    }

}