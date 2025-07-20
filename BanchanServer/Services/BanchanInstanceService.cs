using BanchanServer.Models.Entities;
using BanchanServer.Database.Repositories.Interfaces;

namespace BanchanServer.Services;

public class BanchanInstanceService(IBanchanRepository banchanRepository, IBanchanInstanceRepository banchanInstanceRepository)
{
    private readonly IBanchanRepository _banchanRepository = banchanRepository;
    private readonly IBanchanInstanceRepository _banchanInstanceRepository = banchanInstanceRepository;

    public async Task<BanchanInstance> CreateInstance(string banchanId, string? memo)
    {
        if (string.IsNullOrWhiteSpace(banchanId))
        {
            throw new ArgumentException("반찬 ID는 비어있을 수 없습니다.");
        }

        var banchan = await _banchanRepository.GetByIdAsync(banchanId);

        if (banchan is null)
        {
            throw new InvalidOperationException("해당하는 반찬이 존재하지 않습니다. 반찬을 먼저 등록하세요.");
        }

        var instance = new BanchanInstance 
        {
            Id = Guid.NewGuid().ToString(),
            BanchanId = banchanId,
            Memo = memo
        };

        await _banchanInstanceRepository.AddAsync(instance);

        return instance;
    }

    public Task<BanchanInstance?> GetInstanceById(string id) => _banchanInstanceRepository.GetByIdAsync(id);

    public Task<IEnumerable<BanchanInstance>> GetInstances() => _banchanInstanceRepository.GetAllAsync();

    public async Task<BanchanInstance?> UpdateInstanceMemo(string id, string? memo)
    {
        var instance = await _banchanInstanceRepository.GetByIdAsync(id);

        if (instance is null)
        {
            return null;
        }

        instance.Memo = memo;
        await _banchanInstanceRepository.UpdateAsync(instance);

        return instance;
    }

    public async Task<BanchanInstance?> DeleteInstance(string id)
    {
        var instance = await _banchanInstanceRepository.GetByIdAsync(id);

        if (instance is null)
        {
            return null;
        }

        await _banchanInstanceRepository.DeleteAsync(id);

        return instance;
    }
}