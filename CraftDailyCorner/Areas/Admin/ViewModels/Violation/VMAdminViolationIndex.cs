namespace CraftDailyCorner.Areas.Admin.ViewModels.Violation
{
    public class VMAdminViolationIndex
    {
        // "pending" / "history"
        public string Mode { get; set; } = "pending";

        // History 搜尋用（MemberID：檢舉人或被檢舉者任一命中都顯示）
        public string? SearchMemberId { get; set; }

        // 分頁資訊
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 8;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public List<VMAdminViolationListItem> Items { get; set; } = new();
    }

    public class VMAdminViolationListItem
    {
        public long ReportID { get; set; }

        public byte StatusID { get; set; }
        public string StatusName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        // 檢舉人
        public string ReporterMemberID { get; set; } = null!;
        public string ReporterName { get; set; } = null!;

        // 被檢舉人（由 Target 推得的擁有者）
        public string? TargetOwnerID { get; set; }
        public string? TargetOwnerName { get; set; }

        // 目標
        public byte TargetType { get; set; }
        public string TargetTypeName { get; set; } = null!;
        public string TargetID { get; set; } = null!;

        // 原因
        public int Reason { get; set; }
        public string ReasonName { get; set; } = null!;
    }
}