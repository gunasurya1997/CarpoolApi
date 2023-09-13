using AutoMapper;
using CarpoolService.Common.Exceptions;
using CarpoolService.Contracts;
using CarPoolService.Contracts.Interfaces.Repository_Interfaces;
using CarPoolService.Contracts.Interfaces.Service_Interface;
using CarPoolService.DAL;
using CarPoolService.Models;
using CarPoolService.Models.DBModels;
using Microsoft.EntityFrameworkCore;

namespace CarpoolService.BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly CarpoolDbContext _dbContext;
        private readonly IBCryptService _bcrypt;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, CarpoolDbContext dbContext, IBCryptService bcrypt,IMapper mapper)
        {
            _userRepository = userRepository;
            _dbContext = dbContext;
            _bcrypt = bcrypt;
            _mapper = mapper;
        }

        public async Task<UserDTO> RegisterUserAsync(User user)
        {
            string hashedPassword = _bcrypt.HashPassword(user.Password);
            int highestUserId = await _dbContext.Users.CountAsync();
            User userEntity = new()
            {
                UserId = highestUserId + 1,
                Email = user.Email,
                Password = hashedPassword,
                UserName = user.UserName,
                Image = user.Image
            };
            return await _userRepository.RegisterUser(userEntity);
        }

        public async Task<UserDTO> UpdateUserAsync(int userId, User updatedUser)
        {
            User? existingUser = await _dbContext.Users.FindAsync(userId) ?? throw new NotFoundException();
            existingUser.Email = updatedUser.Email;
            existingUser.UserName = updatedUser.UserName;
            existingUser.Image = updatedUser.Image;

            return await _userRepository.UpdateUser(existingUser);
        }

        public async Task<UserDTO> AuthenticateUserAsync(Login loginUser)
        {
            try
            {
                User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginUser.Email) ?? throw new NotFoundException();

                if (!_bcrypt.VerifyPassword(loginUser.Password, user.Password))
                {
                    throw new UnauthorizedAccessException("Invalid password.");
                }

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user.", ex);
            }
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserById(userId);
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await _userRepository.IsEmailTaken(email);
        }
    }
}