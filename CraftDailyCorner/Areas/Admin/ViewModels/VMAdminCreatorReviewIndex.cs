namespace CraftDailyCorner.Areas.Admin.ViewModels.CreatorReview
{
    public class VMAdminCreatorReviewIndex
    {
        // "pending" / "history"
        public string Mode { get; set; } = "pending";

        // History 搜尋用（MemberID）
        public string? SearchMemberId { get; set; }
        // 分頁資訊
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 8;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public List<VMAdminCreatorReviewListItem> Items { get; set; } = new();
    }

    public class VMAdminCreatorReviewListItem
    {
        public int ApplicationID { get; set; }

        public string MemberID { get; set; } = null!;
        public string MemberName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public string BrandName { get; set; } = null!;
        public DateTime AppliedAt { get; set; }

        public byte StatusID { get; set; }
        public string StatusName { get; set; } = null!;

        

    }
}