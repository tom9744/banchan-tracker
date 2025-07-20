using System.ComponentModel.DataAnnotations;
using BanchanServer.Models.Entities;

namespace BanchanServer.Models.Dtos;

public class BanchanInstanceLogDto
{
    public required string Id { get; set; }
    public required string BanchanInstanceId { get; set; }
    public required LogType Type { get; set; }
    public required string DetailJson { get; set; }
    public required DateTime LoggedAt { get; set; }
}

public class ConsumptionLogRequest
{
    [Required(ErrorMessage = "Portion는 필수 입력 항목입니다.")]
    [Range(0.01, 1.0, ErrorMessage = "Portion는 0.01 이상 1.0 이하여야 합니다.")]
    public double Portion { get; set; }
}