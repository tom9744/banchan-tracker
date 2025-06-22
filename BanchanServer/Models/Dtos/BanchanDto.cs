using System.ComponentModel.DataAnnotations;

namespace BanchanServer.Models.Dtos;

public class BanchanDto 
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required DateTime CreatedAt { get; set; }
}

public class CreateBanchanDto
{
    [Required]
    public required string Name { get; set; }
}

public class UpdateBanchanDto
{
    [Required]
    public required string Name { get; set; }
}   