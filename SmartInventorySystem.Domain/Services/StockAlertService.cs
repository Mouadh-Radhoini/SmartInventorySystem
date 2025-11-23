using SmartInventorySystem.Domain.Entities;
using SmartInventorySystem.Domain.Interfaces;

namespace SmartInventorySystem.Domain.Services
{
    public class StockAlertService
    {
        private readonly IProductRepository _productRepository;

        public StockAlertService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // NEW — Unified method
        public async Task<List<Product>> GetLowStockProductsAsync()
        {
            return await _productRepository.GetLowStockProductsAsync();
        }

        public async Task<List<Product>> GetCriticalStockAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Where(p => p.Quantity <= 2).ToList();
        }
    }
}