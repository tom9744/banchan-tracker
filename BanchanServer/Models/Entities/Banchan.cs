using BanchanServer.Models.Dtos;

namespace BanchanServer.Models.Entities;

public class Banchan
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public BanchanDto ToDto() => new()
    {
        Id = Id,
        Name = Name,
        CreatedAt = CreatedAt
    };
}