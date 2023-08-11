using AutoMapper;
using CarpoolService.Contracts;
using CarPoolService.DAL;
using CarPoolService.Models.DBModels;
using CarPoolService.Models.Interfaces;
using CarPoolService.Models.Interfaces.Repository_Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarpoolService.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CarpoolDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IBCrypt _bcrypt;

        public UserRepository(CarpoolDbContext dbContext, IMapper mapper, IBCrypt bcrypt)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _bcrypt = bcrypt;
        }

        public async Task<UserDto> AddUser(User user)
        {
            string hashedPassword = _bcrypt.HashPassword(user.Password);

            int highestUserId = await _dbContext.Users
          .OrderByDescending(u => u.UserId)
          .Select(u => u.UserId)
          .FirstOrDefaultAsync();

            var userEntity = new User
            {
                UserId = highestUserId + 1, // Increment the UserId
                Email = user.Email,
                Password = hashedPassword,
                UserName = user.UserName,
                Image = user.Image
            };

            _dbContext.Users.Add(userEntity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserDto>(userEntity);
        }
        public async Task<UserDto> UpdateUser(int userId, User updatedUser)
        {
            var existingUser = await _dbContext.Users.FindAsync(userId);

            if (existingUser == null)
            {
                // Handle user not found scenario
                return null;
            }

            existingUser.Email = updatedUser.Email;
            existingUser.UserName = updatedUser.UserName;
            existingUser.Image = updatedUser.Image;

            if (!string.IsNullOrEmpty(updatedUser.Password))
            {
                string hashedPassword = _bcrypt.HashPassword(updatedUser.Password);
                existingUser.Password = hashedPassword;
            }

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserDto>(existingUser);
        }

    }
}