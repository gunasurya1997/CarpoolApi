using CarpoolService.Contracts;
using CarPoolService.Models;
using CarPoolService.Models.DBModels;
using CarPoolService.Models.Interfaces.Service_Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CarPoolServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarpoolController : ControllerBase
    {
        private readonly ICarpoolService _rideService;

        public CarpoolController(ICarpoolService rideService)
        {
            _rideService = rideService;
        }

        [HttpPost("offer-ride")]
        public async Task<GenericApiResponse<CarPoolRideDTO>> OfferRide([FromBody] CarPoolRide ride)
        {
            try
            {
                CarPoolRideDTO offeredRide = await _rideService.OfferRide(ride);
                return new GenericApiResponse<CarPoolRideDTO>().CreateApiResponse(true, HttpStatusCode.OK, offeredRide);
            }
            catch (Exception ex)
            {
                return new GenericApiResponse<CarPoolRideDTO>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        [HttpGet("offered-rides/{userId}")]
        public async Task<GenericApiResponse<List<BookingDTO>>> GetOfferedRides(int userId)
        {
            try
            {
                List<BookingDTO> offeredRides = await _rideService.GetOfferedRides(userId);
                return new GenericApiResponse<List<BookingDTO>>().CreateApiResponse(true, HttpStatusCode.OK, offeredRides);
            }
            catch (Exception ex)
            {
                return new GenericApiResponse<List<BookingDTO>>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        [HttpPost("book-ride")]
        public async Task<GenericApiResponse<BookingDTO>> BookRide([FromBody] Booking booking)
        {
            try
            {
                BookingDTO bookedRide = await _rideService.BookRide(booking);
                return new GenericApiResponse<BookingDTO>().CreateApiResponse(true, HttpStatusCode.OK, bookedRide);
            }
            catch (Exception ex)
            {
                return new GenericApiResponse<BookingDTO>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }
        [HttpGet("booked-rides/{userId}")]
        public async Task<GenericApiResponse<List<CarPoolRideDTO>>> GetBookedRides(int userId)
        {
            try
            {
                List<CarPoolRideDTO> bookedRides = await _rideService.GetBookedRides(userId);
                return new GenericApiResponse<List<CarPoolRideDTO>>().CreateApiResponse(true, HttpStatusCode.OK, bookedRides);
            }
            catch (Exception ex)
            {
                return new GenericApiResponse<List<CarPoolRideDTO>>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        [HttpPost("match-rides")]
        public async Task<GenericApiResponse<List<CarPoolRideDTO>>> MatchRides([FromBody] Ride ride)
        {
            try
            {
                List<CarPoolRideDTO> matchedRides = await _rideService.MatchRides(ride);
                return new GenericApiResponse<List<CarPoolRideDTO>>().CreateApiResponse(true, HttpStatusCode.OK, matchedRides);
            }
            catch (Exception ex)
            {
                return new GenericApiResponse<List<CarPoolRideDTO>>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        [HttpGet("cities")]
        public async Task<GenericApiResponse<List<CityDTO>>> GetCities()
        {
            try
            {
                List<CityDTO> cities = await _rideService.GetCities();
                return new GenericApiResponse<List<CityDTO>>().CreateApiResponse(true, HttpStatusCode.OK, cities);
            }
            catch (Exception ex)
            {
                return new GenericApiResponse<List<CityDTO>>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

    }
}