using Microsoft.EntityFrameworkCore;
using SmartInventorySystem.Domain.Entities;
using SmartInventorySystem.Domain.Interfaces;
using SmartInventorySystem.Infrastructure.Database;

namespace SmartInventorySystem.Infrastructure.Repositories
{
    public class StockMovementRepository : IStockMovementRepository
    {
        private readonly AppDbContext _context;

        public StockMovementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(StockMovement movement)
        {
            _context.StockMovements.Add(movement);
            await _context.SaveChangesAsync();
        }

        public async Task<List<StockMovement>> GetByProductIdAsync(int productId)
        {
            return await _context.StockMovements
                .Where(m => m.ProductId == productId)
                .ToListAsync();
        }
    }
}