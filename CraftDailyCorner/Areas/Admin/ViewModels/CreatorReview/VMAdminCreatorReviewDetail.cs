namespace CraftDailyCorner.Areas.Admin.ViewModels.CreatorReview
{
    public class VMAdminCreatorReviewDetail
    {
        // ===== 申請主資訊 =====
        public int ApplicationID { get; set; }

        public int StatusID { get; set; }
        public string StatusName { get; set; } = null!;

        public DateTime AppliedAt { get; set; }

        // ===== 申請人（會員） =====
        public string MemberID { get; set; } = null!;
        public string MemberName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }

        // ===== 申請內容 =====
        public string? BrandName { get; set; }
        public string? BrandIntro { get; set; }
        public string? PortfolioUrl { get; set; }
        public DateTime? StartDate { get; set; }

        // ===== 審核資訊 =====
        public DateTime? ReviewedAt { get; set; }
        public string? ReviewedBy { get; set; }      // Admin MemberID
        public string? ReviewerName { get; set; }    // Admin DisplayName
        public string? ReviewNote { get; set; }      // 通過備註 or 未通過原因
    }
}