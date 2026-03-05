using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.DTOs
{
    public class ReportDTO
    {
        [Required]
        public ReportTargetType ReportType { get; set; }

        [Required]
        [StringLength(36)]
        public string TargetID { get; set; } = null!;

        public ReportReason ReasonCode { get; set; }

        [StringLength(200, ErrorMessage = "原因不可超過 200 字")]
        public string? Reason { get; set; }
    }
}