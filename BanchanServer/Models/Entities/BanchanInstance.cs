using BanchanServer.Models.Dtos;

namespace BanchanServer.Models.Entities;

public class BanchanInstance
{
    public required string Id { get; set; }
    public required string BanchanId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? Memo { get; set; }

    public BanchanInstanceDto ToDto() => new()
    {
        Id = Id,
        BanchanId = BanchanId,
        Memo = Memo,
        CreatedAt = CreatedAt
    };
}