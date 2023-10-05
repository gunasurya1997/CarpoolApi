using CarPoolService.Models.DBModels;
using DTO = CarpoolService.Contracts.DTOs;


namespace CarpoolService.Contracts.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<DTO.User> RegisterUser(User user);
        Task<DTO.User> UpdateUser(User updatedUser);
        Task<DTO.User> GetUserById(int userId);
        Task<IEnumerable<DTO.User>> GetAllUsers();
        Task<int> GetUserCount();
    }
}
