using BanchanServer.Models.Entities;

namespace BanchanServer.Database.Repositories.Interfaces;

public interface IBanchanInstanceRepository
{
    Task<IEnumerable<BanchanInstance>> GetAllAsync();
    Task<BanchanInstance?> GetByIdAsync(string id);
    Task AddAsync(BanchanInstance banchan);
    Task UpdateAsync(BanchanInstance banchan);
    Task DeleteAsync(string id);
}