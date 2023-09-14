using AutoMapper;
using CarpoolService.Contracts;
using CarPoolService.Contracts.Interfaces.Repository_Interfaces;
using CarPoolService.DAL;
using CarPoolService.Models.DBModels;
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
        public async Task<CarPoolRideDTO> CreateOfferRide(CarPoolRide poolRide)
        {
            try
            {
                await _dbContext.CarPoolRides.AddAsync(poolRide);
                await _dbContext.SaveChangesAsync();
                return _mapper.Map<CarPoolRideDTO>(poolRide);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while creating an offer ride. Details: " + e.Message, e);
            }
        }

        // Method for getting offered rides for a user
        public async Task<IEnumerable<BookingDTO>> GetAllBookedRidesForUser(int userId)
        {
            try
            {
                IEnumerable<Booking> allBookedRides = await _dbContext.Bookings.ToListAsync();
                return _mapper.Map<IEnumerable<BookingDTO>>(allBookedRides);

            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving booked rides: " + e.Message, e);
            }
        }

        // Method for booking a ride
        public async Task<BookingDTO> CreateBookRide(Booking bookRide)
        {
            try
            {
                await _dbContext.Bookings.AddAsync(bookRide);
                await _dbContext.SaveChangesAsync();
                return _mapper.Map<BookingDTO>(bookRide);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while booking a ride: " + e.Message, e);
            }
        }

        // Method for getting booked rides for a user
        public async Task<IEnumerable<CarPoolRideDTO>> GetAllOfferedRidesForUser(int userId)
        {
            try
            {
                IEnumerable<CarPoolRide> allOfferedRides = await _dbContext.CarPoolRides.ToListAsync(); 
                return _mapper.Map<IEnumerable<CarPoolRideDTO>>(allOfferedRides);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving offered rides: " + e.Message,e);
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
                throw new Exception("An error occurred while retrieving cities: " + e.Message,e);
            }
        }

    }

}