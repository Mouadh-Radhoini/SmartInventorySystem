using SmartInventorySystem.Domain.Entities;

namespace SmartInventorySystem.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task<List<User>> GetAllAsync();
    }
}