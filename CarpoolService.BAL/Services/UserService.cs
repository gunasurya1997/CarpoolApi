using AutoMapper;
using CarpoolService.Common.Exceptions;
using CarpoolService.Contracts;
using CarPoolService.Contracts.Interfaces.Repository_Interfaces;
using CarPoolService.Contracts.Interfaces.Service_Interface;
using CarPoolService.Models;
using CarPoolService.Models.DBModels;

namespace CarpoolService.BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBCryptService _bcrypt;
        private readonly IMapper _mapper;
        private IEnumerable<UserDTO> _users; 


        public UserService(IUserRepository userRepository, IBCryptService bcrypt, IMapper mapper)
        {
            _userRepository = userRepository;
            _bcrypt = bcrypt;
            _mapper = mapper;
        }

        private async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            _users ??= await _userRepository.GetAllUsers();
            return _users;
        }

        public async Task<UserDTO> RegisterUserAsync(User user)
        {
            try
            {
                string hashedPassword = _bcrypt.HashPassword(user.Password);
                int highestUserId = (await GetUsersAsync()).Count();
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
            catch (Exception ex)
            {
                throw new Exception("Error registering user.", ex);
            }
        }

        public async Task<UserDTO> UpdateUserAsync(int userId, User updatedUser)
        {
            try
            {
             
                UserDTO existingUserDTO = await GetUserByIdAsync(userId) ?? throw new NotFoundException();
                existingUserDTO.Email = updatedUser.Email;
                existingUserDTO.UserName = updatedUser.UserName;
                existingUserDTO.Image = updatedUser.Image;
                User existingUser = _mapper.Map<User>(existingUserDTO);
                return await _userRepository.UpdateUser(existingUser);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating user.", ex);
            }
        }

        public async Task<UserDTO> AuthenticateUserAsync(Login loginUser)
        {
            try
            {
                IEnumerable<UserDTO> users = await GetUsersAsync();
                UserDTO user = users.FirstOrDefault(u => u.Email == loginUser.Email) ?? throw new NotFoundException();

                if (!_bcrypt.VerifyPassword(loginUser.Password, user.Password))
                {
                    throw new UnauthorizedAccessException("Invalid password.");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user.", ex);
            }
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            try
            {
                return await _userRepository.GetUserById(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting user by ID.", ex);
            }
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            try
            {
                IEnumerable<UserDTO> users = await GetUsersAsync();
                return users.Any(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking if email is taken.", ex);
            }
        }
    }
}
