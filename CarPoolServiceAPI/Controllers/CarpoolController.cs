using CarpoolService.Contracts;
using CarPoolService.Contracts.Interfaces.Service_Interface;
using CarPoolService.Models;
using CarPoolService.Models.DBModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CarPoolServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CarpoolController : ControllerBase
    {
        private readonly ICarpoolService _rideService;

        public CarpoolController(ICarpoolService rideService)
        {
            _rideService = rideService;
        }

        // Create a new offer ride via POST request
        [HttpPost("createOfferRide")]
        public async Task<ApiResponse<CarPoolRideDTO>> CreateOfferRide([FromBody] CarPoolRide ride)
        {
            try
            {
                CarPoolRideDTO offeredRide = await _rideService.CreateOfferRide(ride);
                return new ApiResponse<CarPoolRideDTO>().CreateApiResponse(true, HttpStatusCode.OK, offeredRide);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CarPoolRideDTO>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        // Get offered rides for a user via GET request
        [HttpGet("getOfferedRides/{userId}")]
        public async Task<ApiResponse<IEnumerable<BookingDTO>>> GetOfferedRides([FromRoute] int userId)
        {
            try
            {
                IEnumerable<BookingDTO> offeredRides = await _rideService.GetOfferedRides(userId);
                return new ApiResponse<IEnumerable<BookingDTO>>().CreateApiResponse(true, HttpStatusCode.OK, offeredRides);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<BookingDTO>>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }               
        }

        // Create a new booked ride via POST request
        [HttpPost("createBookRide")]
        public async Task<ApiResponse<BookingDTO>> CreateBookRide([FromBody] Booking booking)
        {
            try
            {
                BookingDTO bookedRide = await _rideService.CreateBookRide(booking);
                return new ApiResponse<BookingDTO>().CreateApiResponse(true, HttpStatusCode.OK, bookedRide);
            }
            catch (Exception ex)
            {
                return new ApiResponse<BookingDTO>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        // Get booked rides for a user via GET request
        [HttpGet("getBookedRides/{userId}")]
        public async Task<ApiResponse<IEnumerable<CarPoolRideDTO>>> GetBookedRides([FromRoute] int userId)
        {
            try
            {
                IEnumerable<CarPoolRideDTO> bookedRides = await _rideService.GetBookedRides(userId);
                return new ApiResponse<IEnumerable<CarPoolRideDTO>>().CreateApiResponse(true, HttpStatusCode.OK, bookedRides);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<CarPoolRideDTO>>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        // Find matching rides for a given ride via POST request
        [HttpPost("match-rides")]
        public async Task<ApiResponse<IEnumerable<CarPoolRideDTO>>> FindMatchingRides([FromBody] Ride ride)
        {
            try
            {
                IEnumerable<CarPoolRideDTO> matchedRides = await _rideService.FindMatchRides(ride);
                return new ApiResponse<IEnumerable<CarPoolRideDTO>>().CreateApiResponse(true, HttpStatusCode.OK, matchedRides);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<CarPoolRideDTO>>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        // Get a list of cities via GET request
        [HttpGet("cities")]
        public async Task<ApiResponse<IEnumerable<CityDTO>>> GetCities()
        {
            try
            {
                IEnumerable<CityDTO> cities = await _rideService.GetCities();
                return new ApiResponse<IEnumerable<CityDTO>>().CreateApiResponse(true, HttpStatusCode.OK, cities);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<CityDTO>>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

    }
}