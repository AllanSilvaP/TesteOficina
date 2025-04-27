using Microsoft.EntityFrameworkCore;
using RO.DevTest.Application.Interfaces;

namespace RO.DevTest.Persistence.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _context;

    public SaleRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Sale>> GetAllAsync()
    {
        return await _context.Sales
            .Include(s => s.saleItems)
            .ThenInclude(si => si.Product) // carrega também o produto
            .ToListAsync();
    }

    public async Task<Sale?> GetByIdAsync(Guid id) => await _context.Sales.FindAsync(id);
    public async Task AddAsync(Sale sale)
    {
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Sale sale)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var sale = await _context.Sales.FindAsync(id);
        if (sale is not null)
        {
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
        }
    }
}