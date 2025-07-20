using BanchanServer.Models.Dtos;

namespace BanchanServer.Models.Entities;

public enum LogType {
    Consumption,
    Disposal
}

public class BanchanInstanceLog
{
    public required string Id { get; set; }
    public required string BanchanInstanceId { get; set; }
    public required LogType Type { get; set; }
    public string DetailJson { get; set; } = string.Empty;
    public DateTime LoggedAt { get; set; }

    public BanchanInstanceLogDto ToDto() => new()
    {
        Id = Id,
        BanchanInstanceId = BanchanInstanceId,
        Type = Type,
        DetailJson = DetailJson,
        LoggedAt = LoggedAt
    };
}