using CarpoolService.Contracts;
using CarPoolService.Models;
using CarPoolService.Models.DBModels;
using CarPoolService.Models.Interfaces.Service_Interface;
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
        public async Task<GenericApiResponse<IEnumerable<BookingDTO>>> GetOfferedRides([FromRoute] int userId)
        {
            try
            {
                IEnumerable<BookingDTO> offeredRides = await _rideService.GetOfferedRides(userId);
                return new GenericApiResponse<IEnumerable<BookingDTO>>().CreateApiResponse(true, HttpStatusCode.OK, offeredRides);
            }
            catch (Exception ex)
            {
                return new GenericApiResponse<IEnumerable<BookingDTO>>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
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
        public async Task<GenericApiResponse<IEnumerable<CarPoolRideDTO>>> GetBookedRides([FromRoute] int userId)
        {
            try
            {
                IEnumerable<CarPoolRideDTO> bookedRides = await _rideService.GetBookedRides(userId);
                return new GenericApiResponse<IEnumerable<CarPoolRideDTO>>().CreateApiResponse(true, HttpStatusCode.OK, bookedRides);
            }
            catch (Exception ex)
            {
                return new GenericApiResponse<IEnumerable<CarPoolRideDTO>>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        [HttpPost("match-rides")]
        public async Task<GenericApiResponse<IEnumerable<CarPoolRideDTO>>> MatchRides([FromBody] Ride ride)
        {
            try
            {
                IEnumerable<CarPoolRideDTO> matchedRides = await _rideService.MatchRides(ride);
                return new GenericApiResponse<IEnumerable<CarPoolRideDTO>>().CreateApiResponse(true, HttpStatusCode.OK, matchedRides);
            }
            catch (Exception ex)
            {
                return new GenericApiResponse<IEnumerable<CarPoolRideDTO>>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        [HttpGet("cities")]
        public async Task<GenericApiResponse<IEnumerable<CityDTO>>> GetCities()
        {
            try
            {
                IEnumerable<CityDTO> cities = await _rideService.GetCities();
                return new GenericApiResponse<IEnumerable<CityDTO>>().CreateApiResponse(true, HttpStatusCode.OK, cities);
            }
            catch (Exception ex)
            {
                return new GenericApiResponse<IEnumerable<CityDTO>>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

    }
}