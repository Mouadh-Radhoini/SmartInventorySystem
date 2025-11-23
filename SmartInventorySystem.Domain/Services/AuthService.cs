using SmartInventorySystem.Domain.Entities;
using SmartInventorySystem.Domain.Interfaces;

namespace SmartInventorySystem.Domain.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepo;

        public AuthService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _userRepo.GetByUsernameAsync(username);

            if (user == null) 
                return null;

            if (user.Password != password)
                return null;

            return user;
        }
    }
}