using AutoMapper;
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

        [HttpPost("register")]
        public async Task<GenericApiResponse<UserDto>> AddUser(User user)
        {
            try
            {
                bool emailExists = await _userService.IsEmailTakenAsync(user.Email);

                if (emailExists)
                {
                    return new GenericApiResponse<UserDto>().CreateApiResponse(false, HttpStatusCode.BadRequest, null, "Email already taken");
                }

                UserDto createdUser = await _userService.CreateUserAsync(user);
                UserDto userDto = _mapper.Map<UserDto>(createdUser);
                return new GenericApiResponse<UserDto>().CreateApiResponse(true, HttpStatusCode.OK, userDto);
            }
            catch (Exception ex)
            {
                return new GenericApiResponse<UserDto>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, "Failed to Add User!!!");
            }
        }


        [HttpPut("register/{userId}")]
        public async Task<GenericApiResponse<UserDto>> UpdateUser(int userId, User user)
        {
            try
            {
                UserDto updatedUser = await _userService.UpdateUserAsync(userId, user);
                if (updatedUser == null)
                {
                    return new GenericApiResponse<UserDto>().CreateApiResponse(false, HttpStatusCode.NotFound, null, "User not found");
                }
                return new GenericApiResponse<UserDto>().CreateApiResponse(true, HttpStatusCode.OK, updatedUser);
            }
            catch (Exception ex)
            {
                return new GenericApiResponse<UserDto>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        [HttpGet("{userId}")]
        public async Task<GenericApiResponse<UserDto>> GetUserById(int userId)
        {
            try
            {
                UserDto user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return new GenericApiResponse<UserDto>().CreateApiResponse(false, HttpStatusCode.NotFound, null, "User not found");
                }
                return new GenericApiResponse<UserDto>().CreateApiResponse(true, HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                return new GenericApiResponse<UserDto>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<GenericApiResponse<string>> Login(Login user)
        {
            try
            {
                UserDto authenticatedUser = await _userService.AuthenticateUserAsync(user);

                if (authenticatedUser == null)
                {
                    return new GenericApiResponse<string>().CreateApiResponse(false, HttpStatusCode.BadRequest, null, "User not Found");
                }
                var token = _tokenService.GenerateToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    _config["Jwt:Key"],
                    authenticatedUser
                );
                return new GenericApiResponse<string>().CreateApiResponse(true, HttpStatusCode.OK, token);
            }
            catch (Exception ex)
            {
                return new GenericApiResponse<string>().CreateApiResponse(false, HttpStatusCode.InternalServerError, null, ex.Message);
            }
        }
    }
}
