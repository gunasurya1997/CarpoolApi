using AutoMapper;
using CarpoolService.Contracts;
using CarPoolService.Models.DBModels;
using CarPoolService.Models.Interfaces.Service_Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CarPoolServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> AddUser(User user)
        {
            try
            {
                UserDto createdUser = await _userService.CreateUserAsync(user);
                UserDto userDto = _mapper.Map<UserDto>(createdUser);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred");
            }
        }

        [HttpPut("register/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, User User)
        {
            try
            {
                UserDto updatedUser = await _userService.UpdateUserAsync(userId, User);
                if (updatedUser == null)
                {
                    return NotFound("User not found");
                }
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred");
            }
        }
    }
}
