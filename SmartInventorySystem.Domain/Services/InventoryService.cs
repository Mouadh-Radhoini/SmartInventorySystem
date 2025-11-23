using SmartInventorySystem.Domain.Entities;
using SmartInventorySystem.Domain.Interfaces;

namespace SmartInventorySystem.Domain.Services
{
    public class InventoryService
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockMovementRepository _stockMovementRepository;

        public InventoryService(
            IProductRepository productRepository,
            IStockMovementRepository stockMovementRepository)
        {
            _productRepository = productRepository;
            _stockMovementRepository = stockMovementRepository;
        }

        public async Task AddProductAsync(Product product)
        {
            product.CreatedAt = DateTime.Now;
            await _productRepository.AddAsync(product);

            await _stockMovementRepository.AddAsync(new StockMovement
            {
                ProductId = product.Id,
                Type = "IN",
                Quantity = product.Quantity,
                Date = DateTime.Now
            });
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteAsync(id);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }
    }
}