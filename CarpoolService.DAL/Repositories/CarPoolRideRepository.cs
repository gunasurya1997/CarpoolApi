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
            _ = await _dbContext.CarPoolRides.AddAsync(poolRide);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<CarPoolRideDTO>(poolRide);

        }


        public async Task<List<CarPoolRideDTO>> GetOfferedRidesForUser(int userId)
        {
            try
            {
                List<CarPoolRide> offeredRides = await _dbContext.CarPoolRides
                    .Where(ride => _dbContext.Bookings
                        .Any(booking => booking.PassengerId == userId && booking.RideId == ride.RideId))
                    .ToListAsync();

                return _mapper.Map<List<CarPoolRideDTO>>(offeredRides);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving offered rides: " + e.Message);
            }
        }



        public async Task<BookingDTO> BookRide(Booking bookRide)
        {
            var offeredRide = await _dbContext.CarPoolRides.FirstOrDefaultAsync(x =>
                x.DepartureCityId == bookRide.PickupLocationId &&
                x.DestinationCityId == bookRide.DropLocationId &&
                x.TimeSlot == bookRide.TimeSlot &&
                x.Date == bookRide.Date &&
                x.AvailableSeats >= bookRide.ReservedSeats
            );

            if (offeredRide != null)
            {
                // Update the offered ride details
                offeredRide.AvailableSeats -= bookRide.ReservedSeats;
                if (offeredRide.AvailableSeats == 0)
                {
                    offeredRide.RideStatus = false;
                }

                await _dbContext.Bookings.AddAsync(bookRide);
                await _dbContext.SaveChangesAsync();

                // Update the offered ride in the database
                _dbContext.CarPoolRides.Update(offeredRide);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<BookingDTO>(bookRide);
            }

            throw new Exception("No matching ride available for booking.");
        }

        public async Task<List<BookingDTO>> GetBookedRidesForUser(int userId)
        {
            try
            {
                List<Booking> bookedRides = await _dbContext.Bookings
                    .Where(booking => _dbContext.CarPoolRides
                        .Any(ride => ride.RideId == booking.RideId && ride.DriverId == userId))
                    .ToListAsync();

                return _mapper.Map<List<BookingDTO>>(bookedRides);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving booked rides: " + e.Message);
            }
        }


        public async Task<List<CarPoolRideDTO>> MatchRides(Ride ride)
        {
            List<CarPoolRide> matchedRides = await _dbContext.CarPoolRides
                .Where(cpRide =>
                                 cpRide.DepartureCityId == ride.StartPoint &&
                                 cpRide.DestinationCityId == ride.EndPoint &&
                                 cpRide.Date == ride.Date &&
                                 cpRide.TimeSlot == ride.TimeSlot).ToListAsync();

            return _mapper.Map<List<CarPoolRideDTO>>(matchedRides);
        }

    }
    
}
