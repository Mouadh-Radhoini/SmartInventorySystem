using Microsoft.EntityFrameworkCore;
using SmartInventorySystem.Domain.Entities;
using SmartInventorySystem.Domain.Interfaces;
using SmartInventorySystem.Infrastructure.Database;

namespace SmartInventorySystem.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly AppDbContext _context;

        public SaleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sale>> GetAllAsync()
        {
            return await _context.Sales
                .Include(s => s.Product)
                .ToListAsync();
        }

        public async Task AddAsync(Sale sale)
        {
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
        }
    }
}