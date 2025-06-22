using BanchanServer.Models.Entities;

namespace BanchanServer.Database.Repositories.Interfaces;

public interface IBanchanRepository
{
    Task<IEnumerable<Banchan>> GetAllAsync();
    Task<Banchan?> GetByIdAsync(string id);
    Task AddAsync(Banchan banchan);
    Task UpdateAsync(Banchan banchan);
    Task DeleteAsync(string id);
}