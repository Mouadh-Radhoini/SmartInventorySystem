using SmartInventorySystem.Domain.Entities;

namespace SmartInventorySystem.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);

        // NEW
        Task<List<Product>> GetLowStockProductsAsync();
    }
}