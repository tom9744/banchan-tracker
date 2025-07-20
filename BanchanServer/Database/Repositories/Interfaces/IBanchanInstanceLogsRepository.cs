using BanchanServer.Models.Entities;

namespace BanchanServer.Database.Repositories.Interfaces;

public interface IBanchanInstanceLogRepository
{
    Task AddAsync(BanchanInstanceLog log);
    Task<BanchanInstanceLog?> GetByIdAsync(string id);
    Task<IEnumerable<BanchanInstanceLog>> GetByInstanceIdAsync(string instanceId);
}