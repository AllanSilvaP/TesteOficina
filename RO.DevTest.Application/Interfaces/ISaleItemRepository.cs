namespace RO.DevTest.Application.Interfaces;

public interface ISaleItemRepository {
    Task<IEnumerable<SaleItem>> GetAllAsync();
    Task<SaleItem?> GetByIdAsync(Guid id);
    Task AddAsync(SaleItem saleItem);
    Task UpdateAsync(SaleItem saleItem);
    Task DeleteAsync(Guid id);
}