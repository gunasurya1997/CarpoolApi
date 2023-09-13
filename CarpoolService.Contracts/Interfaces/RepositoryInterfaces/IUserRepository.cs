using CarpoolService.Contracts;
using CarPoolService.Models.DBModels;

namespace CarPoolService.Contracts.Interfaces.Repository_Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO> RegisterUser(User user);
        Task<UserDTO> UpdateUser(User updatedUser);
        Task<UserDTO> GetUserById(int userId);
        Task<bool> IsEmailTaken(string email);
    }
}
