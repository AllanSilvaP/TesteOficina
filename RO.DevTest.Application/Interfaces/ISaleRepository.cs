namespace RO.DevTest.Application.Interfaces;

public interface ISaleRepository {
    Task<IEnumerable<Sale>> GetAllAsync();
    Task<Sale?> GetByIdAsync(Guid id);
    Task AddAsync(Sale sale);
    Task UpdateAsync(Sale sale);
    Task DeleteAsync(Guid id);
}