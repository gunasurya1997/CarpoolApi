using AutoMapper;
using CarpoolService.Common.Exceptions;
using CarpoolService.Contracts;
using CarPoolService.Contracts.Interfaces.Repository_Interfaces;
using CarPoolService.DAL;
using CarPoolService.Models.DBModels;
using Microsoft.EntityFrameworkCore;

namespace CarpoolService.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CarpoolDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserRepository(CarpoolDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // Register a new user
        public async Task<UserDTO> RegisterUser(User user)
        {
            try
            {
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
                return _mapper.Map<UserDTO>(user);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error adding a new user. Database update failed.", ex);
            }
        }

        // Update user information
        public async Task<UserDTO> UpdateUser(User updatedUser)
        {
            try
            {
                _dbContext.Entry(updatedUser).State = EntityState.Modified; 
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<UserDTO>(updatedUser);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error updating user information. Database update failed.", ex);
            }
        }

        // Get a user by ID
        public async Task<UserDTO> GetUserById(int userId)
        {
            try
            {
                User? user = await _dbContext.Users.FindAsync(userId) ?? throw new NotFoundException();
                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting user by ID.", ex);
            }
        }

        // Check if an email is already taken
        public async Task<bool> IsEmailTaken(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

    }
}
