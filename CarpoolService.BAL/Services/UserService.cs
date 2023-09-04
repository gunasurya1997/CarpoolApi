using CarpoolService.Contracts;
using CarPoolService.DAL;
using CarPoolService.Models;
using CarPoolService.Models.DBModels;
using CarPoolService.Models.Interfaces.Repository_Interfaces;
using CarPoolService.Models.Interfaces.Service_Interface;
using Microsoft.EntityFrameworkCore;

namespace CarpoolService.BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly CarpoolDbContext _dbContext;

        public UserService(IUserRepository userRepository, CarpoolDbContext dbContext)
        {
            _userRepository = userRepository;
            _dbContext = dbContext;
        }

        public async Task<UserDto> CreateUserAsync(User user)
        {
            return await _userRepository.AddUser(user);
        }

        public async Task<UserDto> UpdateUserAsync(int userId, User updatedUser)
        {
            return await _userRepository.UpdateUser(userId, updatedUser);
        }

        public async Task<UserDto> AuthenticateUserAsync(Login loginUser)
        {
            return await _userRepository.AuthenticateUser(loginUser);
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserById(userId);
        }
        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }
    }
}