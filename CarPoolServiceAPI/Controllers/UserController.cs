using AutoMapper;
using CarpoolService.Contracts;
using CarPoolService.Contracts.Interfaces.Service_Interface;
using CarPoolService.Models;
using CarPoolService.Models.DBModels;
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
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public UserController(IUserService userService, IMapper mapper, ITokenService tokenService, IConfiguration config)
        {
            _userService = userService;
            _mapper = mapper;
            _tokenService = tokenService;
            _config = config;
        }

        // Register a new user via POST request
        [HttpPost("register")]
        public async Task<ApiResponse<UserDTO>> RegisterUser([FromBody]User user)
        {
            try
            {
                bool emailExists = await _userService.IsEmailTakenAsync(user.Email);

                if (emailExists)
                {
                    return new ApiResponse<UserDTO>().CreateApiResponse(false, HttpStatusCode.BadRequest, null, "Email already taken");
                }

                UserDTO createdUser = await _userService.RegisterUserAsync(user);
                UserDTO userDto = _mapper.Map<UserDTO>(createdUser);
                return new ApiResponse<UserDTO>().CreateApiResponse(true, HttpStatusCode.OK, userDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserDTO>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null,ex.Message);
            }
        }

        // Update user information via PUT request
        [HttpPut("update/{userId}")]
        public async Task<ApiResponse<UserDTO>> UpdateUser([FromRoute] int userId, [FromBody]User user)
        {
            try
            {
                UserDTO updatedUser = await _userService.UpdateUserAsync(userId, user);
                if (updatedUser == null)
                {
                    return new ApiResponse<UserDTO>().CreateApiResponse(false, HttpStatusCode.NotFound, null, "User not found");
                }
                return new ApiResponse<UserDTO>().CreateApiResponse(true, HttpStatusCode.OK, updatedUser);
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserDTO>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        // Get user by ID via GET request
        [HttpGet("{userId}")]
        public async Task<ApiResponse<UserDTO>> GetUserById([FromRoute] int userId)
        {
            try
            {
                UserDTO user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponse<UserDTO>().CreateApiResponse(false, HttpStatusCode.NotFound, null, "User not found");
                }
                return new ApiResponse<UserDTO>().CreateApiResponse(true, HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserDTO>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        // Authenticate user via POST request
        [HttpPost("login")]
        public async Task<ApiResponse<string>> Login([FromBody]Login user)
        {
            try
            {
                UserDTO authenticatedUser = await _userService.AuthenticateUserAsync(user);

                if (authenticatedUser == null)
                {
                    return new ApiResponse<string>().CreateApiResponse(false, HttpStatusCode.BadRequest, null, "User not Found");
                }
                var token = _tokenService.GenerateToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    _config["Jwt:Key"],
                    authenticatedUser
                );
                return new ApiResponse<string>().CreateApiResponse(true, HttpStatusCode.OK, token);
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }
    }
}
