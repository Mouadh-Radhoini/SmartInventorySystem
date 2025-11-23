using SmartInventorySystem.Domain.Entities;
using SmartInventorySystem.Domain.Interfaces;

namespace SmartInventorySystem.Domain.Services
{
    public class SalesService
    {
        private readonly IProductRepository _productRepository;
        private readonly ISaleRepository _saleRepository;

        public SalesService(
            IProductRepository productRepository,
            ISaleRepository saleRepository)
        {
            _productRepository = productRepository;
            _saleRepository = saleRepository;
        }

        public async Task<List<Sale>> GetAllSalesAsync()
        {
            return await _saleRepository.GetAllAsync();
        }

        public async Task MakeSaleAsync(int productId, int quantity)
        {
            // 1) Get product
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found.");

            // 2) Check stock
            if (product.Quantity < quantity)
                throw new Exception("Not enough stock.");

            // 3) Create sale
            var total = product.Price * quantity;

            var sale = new Sale
            {
                ProductId = product.Id,
                Quantity = quantity,
                TotalPrice = total,
                Date = DateTime.UtcNow
            };

            // 4) Save sale
            await _saleRepository.AddAsync(sale);

            // 5) Decrease stock
            product.Quantity -= quantity;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(product);
        }
    }
}