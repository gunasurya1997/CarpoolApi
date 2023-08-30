using CarpoolService.Contracts;
using CarPoolService.Models.DBModels;

namespace CarPoolService.Models.Interfaces.Repository_Interfaces
{
    public interface IUserRepository
    {
        Task<UserDto> AddUser(User user);
        Task<UserDto> UpdateUser(int userId, User updatedUser);
        Task<UserDto> AuthenticateUser(Login loginUser);
        Task<UserDto> GetUserById(int userId);
    }
}
