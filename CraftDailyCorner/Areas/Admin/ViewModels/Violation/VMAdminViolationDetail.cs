namespace CraftDailyCorner.Areas.Admin.ViewModels.Violation
{
    public class VMAdminViolationDetail
    {
        public long ReportID { get; set; }

        public byte StatusID { get; set; }
        public string StatusName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        // 檢舉人
        public string ReporterMemberID { get; set; } = null!;
        public string ReporterName { get; set; } = null!;
        public string? ReporterEmail { get; set; }
        public string? ReporterPhone { get; set; }

        // 目標（被檢舉內容）
        public byte TargetType { get; set; }
        public string TargetTypeName { get; set; } = null!;
        public string TargetID { get; set; } = null!;
        public string? UserReasonText { get; set; }

        // 被檢舉者
        public string? TargetOwnerID { get; set; }
        public string? TargetOwnerName { get; set; }

        // 檢舉原因
        public int Reason { get; set; }
        public string ReasonName { get; set; } = null!;

        // 管理者備註
        public string? AdminNote { get; set; }
        // 被檢舉內容的連結
        public string? TargetUrl { get; set; }
    }
}