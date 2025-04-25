using Microsoft.EntityFrameworkCore;
using RO.DevTest.Application.Interfaces;

namespace RO.DevTest.Persistence.Repositories;

public class SaleItemRepository : ISaleItemRepository {
    private readonly AppDbContext _context;

    public SaleItemRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<SaleItem>> GetAllAsync() => await _context.SaleItems.ToListAsync();
    public async Task<SaleItem?> GetByIdAsync(Guid id) => await _context.SaleItems.FindAsync(id);
    public async Task AddAsync(SaleItem saleItem) {
        _context.SaleItems.Add(saleItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SaleItem saleItem) {
        _context.SaleItems.Update(saleItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id) {
        var saleItem = await _context.SaleItems.FindAsync(id);
        if (saleItem is not null) {
            _context.SaleItems.Remove(saleItem);
            await _context.SaveChangesAsync();
        }
    }
}