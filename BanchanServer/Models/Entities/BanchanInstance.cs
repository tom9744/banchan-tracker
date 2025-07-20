using BanchanServer.Models.Dtos;

namespace BanchanServer.Models.Entities;

public class BanchanInstance
{
    public required string Id { get; set; }
    public required string BanchanId { get; set; }
    public double RemainingPortion { get; set; } = 1;
    public string? Memo { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? FinishedAt { get; set; }

    public BanchanInstanceDto ToDto() => new()
    {
        Id = Id,
        BanchanId = BanchanId,
        RemainingPortion = RemainingPortion,
        Memo = Memo,
        CreatedAt = CreatedAt,
        FinishedAt = FinishedAt
    };
}