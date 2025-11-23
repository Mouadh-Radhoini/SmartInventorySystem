using SmartInventorySystem.Domain.Entities;

namespace SmartInventorySystem.Domain.Interfaces
{
    public interface ISaleRepository
    {
        Task<List<Sale>> GetAllAsync();
        Task AddAsync(Sale sale);
    }
}