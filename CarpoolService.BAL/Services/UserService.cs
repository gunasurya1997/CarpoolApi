using CarpoolService.Contracts;
using CarPoolService.Models.DBModels;
using CarPoolService.Models.Interfaces.Repository_Interfaces;
using CarPoolService.Models.Interfaces.Service_Interface;

namespace CarpoolService.BAL.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> CreateUserAsync(User user)
        {
            return await _userRepository.AddUser(user);
        }
        public async Task<UserDto> UpdateUserAsync(int userId, User updatedUser)
        {
            return await _userRepository.UpdateUser(userId, updatedUser);
        }
    }
}
