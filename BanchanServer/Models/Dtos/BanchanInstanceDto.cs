using System.ComponentModel.DataAnnotations;

namespace BanchanServer.Models.Dtos;

public class BanchanInstanceDto 
{
    public required string Id { get; set; }
    public required string BanchanId { get; set; }
    public required double RemainingPortion { get; set; }
    public required string? Memo { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime? FinishedAt { get; set; }
}

public class CreateBanchanInstanceDto
{
    [Required(ErrorMessage = "BanchanId는 필수 입력 항목입니다.")]
    public required string BanchanId { get; set; }
    [MaxLength(100, ErrorMessage = "메모는 100자 이하여야 합니다.")]
    public string? Memo { get; set; }
}

public class UpdateBanchanInstanceDto
{
    [MaxLength(100, ErrorMessage = "메모는 100자 이하여야 합니다.")]
    public string? Memo { get; set; }
}   