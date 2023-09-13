using CarpoolService.Contracts;
using CarPoolService.Models;
using CarPoolService.Models.DBModels;

namespace CarPoolService.Contracts.Interfaces.Service_Interface
{
    public interface IUserService
    {
        Task<UserDTO> RegisterUserAsync(User user);
        Task<UserDTO> UpdateUserAsync(int userId, User updatedUser);
        Task<UserDTO> AuthenticateUserAsync(Login loginUser);
        Task<UserDTO> GetUserByIdAsync(int userId);

        Task<bool> IsEmailTakenAsync(string email);
    }
}
