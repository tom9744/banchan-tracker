using BanchanServer.Models.Entities;
using BanchanServer.Database.Repositories.Interfaces;
using System.Text.Json;

namespace BanchanServer.Services;

public class BanchanInstanceLogService(
    IBanchanInstanceRepository banchanInstanceRepository,
    IBanchanInstanceLogRepository banchanInstanceLogRepository)
{
    private readonly IBanchanInstanceRepository _banchanInstanceRepository = banchanInstanceRepository;
    private readonly IBanchanInstanceLogRepository _logRepository = banchanInstanceLogRepository;

    public async Task<BanchanInstanceLog> ProcessConsumption(string instanceId, double portion)
    {
        if (string.IsNullOrWhiteSpace(instanceId))
        {
            throw new ArgumentException("인스턴스 ID는 비어있을 수 없습니다.");
        }

        if (portion <= 0 || portion > 1)
        {
            throw new ArgumentException("Portion은 0보다 크고 1 이하여야 합니다.");
        }

        var instance = await _banchanInstanceRepository.GetByIdAsync(instanceId);
        
        if (instance is null)
        {
            throw new InvalidOperationException("해당하는 반찬 인스턴스가 존재하지 않습니다.");
        }

        if (instance.FinishedAt is not null)
        {
            throw new InvalidOperationException("이미 소비가 완료된 반찬 인스턴스입니다.");
        }

        var existingLogs = await GetLogsByInstanceId(instanceId);

        if (existingLogs.Any(log => log.Type == LogType.Disposal))
        {
            throw new InvalidOperationException("이미 폐기된 반찬 인스턴스입니다.");
        }

        var log = new BanchanInstanceLog
        {
            Id = Guid.NewGuid().ToString(),
            BanchanInstanceId = instanceId,
            Type = LogType.Consumption,
            DetailJson = JsonSerializer.Serialize(new { Portion = portion }),
            LoggedAt = DateTime.UtcNow
        };

        Console.WriteLine($"ProcessConsumption: {instanceId}, {portion}");
        Console.WriteLine($"Log: {log.Id}, {log.BanchanInstanceId}, {log.Type}, {log.DetailJson}, {log.LoggedAt}");

        await _logRepository.AddAsync(log);

        instance.RemainingPortion -= instance.RemainingPortion * portion;
        instance.UpdatedAt = DateTime.UtcNow;
        if (instance.RemainingPortion <= 0)
        {
            instance.FinishedAt = DateTime.UtcNow;
        }

        await _banchanInstanceRepository.UpdateAsync(instance);

        return log;
    }

    public async Task<BanchanInstanceLog> ProcessDisposal(string instanceId)
    {
        if (string.IsNullOrWhiteSpace(instanceId))
        {
            throw new ArgumentException("인스턴스 ID는 비어있을 수 없습니다.");
        }

        var instance = await _banchanInstanceRepository.GetByIdAsync(instanceId);
        
        if (instance is null)
        {
            throw new InvalidOperationException("해당하는 반찬 인스턴스가 존재하지 않습니다.");
        }

        if (instance.FinishedAt is not null)
        {
            throw new InvalidOperationException("이미 소비가 완료된 반찬 인스턴스입니다.");
        }

        var existingLogs = await GetLogsByInstanceId(instanceId);

        if (existingLogs.Any(log => log.Type == LogType.Disposal))
        {
            throw new InvalidOperationException("이미 폐기된 반찬 인스턴스입니다.");
        }

        var log = new BanchanInstanceLog
        {
            Id = Guid.NewGuid().ToString(),
            BanchanInstanceId = instanceId,
            Type = LogType.Disposal,
            DetailJson = JsonSerializer.Serialize(new { }),
            LoggedAt = DateTime.UtcNow
        };

        await _logRepository.AddAsync(log);

        instance.RemainingPortion = 0;
        instance.UpdatedAt = DateTime.UtcNow;
        instance.FinishedAt = DateTime.UtcNow;
        
        await _banchanInstanceRepository.UpdateAsync(instance);

        return log;
    }

    public Task<BanchanInstanceLog?> GetLogById(string id) => _logRepository.GetByIdAsync(id);

    public Task<IEnumerable<BanchanInstanceLog>> GetLogsByInstanceId(string instanceId) => _logRepository.GetByInstanceIdAsync(instanceId);
}