using Microsoft.EntityFrameworkCore;
using RO.DevTest.Application.Interfaces;

namespace RO.DevTest.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Customer>> GetAllAsync() => await _context.Customers.ToListAsync();

    public async Task<Customer?> GetByIdAsync(Guid id) => await _context.Customers.FindAsync(id);

    public async Task AddAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer is not null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}