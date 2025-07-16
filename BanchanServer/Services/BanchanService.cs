using BanchanServer.Models.Entities;
using BanchanServer.Database.Repositories.Interfaces;

namespace BanchanServer.Services;

public class BanchanService(IBanchanRepository banchanRepository)
{
    private readonly IBanchanRepository _banchanRepository = banchanRepository;

    public async Task<Banchan> CreateBanchan(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("반찬 이름은 비어있을 수 없습니다.");
        }

        var banchan = new Banchan 
        {
            Id = Guid.NewGuid().ToString(),
            Name = name
        };

        await _banchanRepository.AddAsync(banchan);

        return banchan;
    }

    public Task<Banchan?> GetBanchanById(string id) => _banchanRepository.GetByIdAsync(id);

    public Task<IEnumerable<Banchan>> GetBanchans() => _banchanRepository.GetAllAsync();

    public async Task<Banchan?> UpdateBanchan(string id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("반찬 이름은 비어있을 수 없습니다.");
        }

        var banchan = await _banchanRepository.GetByIdAsync(id);

        if (banchan is null)
        {
            return null;
        }

        banchan.Name = name;
        await _banchanRepository.UpdateAsync(banchan);

        return banchan;
    }

    public async Task<Banchan?> DeleteBanchan(string id)
    {
        var banchan = await _banchanRepository.GetByIdAsync(id);

        if (banchan is null)
        {
            return null;
        }

        await _banchanRepository.DeleteAsync(id);

        return banchan;
    }
}