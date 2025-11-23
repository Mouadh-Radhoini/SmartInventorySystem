using SmartInventorySystem.Domain.Entities;

namespace SmartInventorySystem.Domain.Interfaces
{
    public interface IStockMovementRepository
    {
        Task AddAsync(StockMovement movement);
        Task<List<StockMovement>> GetByProductIdAsync(int productId);
    }
}