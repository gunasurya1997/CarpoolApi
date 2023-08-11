using CarPoolService.Models;
using CarPoolService.Models.DBModels;
using CarPoolService.Models.Interfaces.Service_Interface;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> OfferRide([FromBody] CarPoolRide ride)
        {
            var offeredRide = await _rideService.OfferRide(ride);
            return Ok(offeredRide);
        }

        [HttpGet("offered-rides/{userId}")]
        public async Task<IActionResult> GetOfferedRides(int userId)
        {
            var offeredRides = await _rideService.GetOfferedRides(userId);
            return Ok(offeredRides);
        }

        [HttpPost("book-ride")]
        public async Task<IActionResult> BookRide([FromBody] Booking booking)
        {
            var bookedRide = await _rideService.BookRide(booking);
            return Ok(bookedRide);
        }
        [HttpGet("booked-rides/{userId}")]
        public async Task<IActionResult> GetBookedRides(int userId)
        {
            var bookedRides = await _rideService.GetBookedRides(userId);
            return Ok(bookedRides);
        }

        [HttpPost("match-rides")]
        public async Task<IActionResult> MatchRides([FromBody] Ride ride)
        {
            var matchedRides = await _rideService.MatchRides(ride);
            return Ok(matchedRides);
        }

    }
}
