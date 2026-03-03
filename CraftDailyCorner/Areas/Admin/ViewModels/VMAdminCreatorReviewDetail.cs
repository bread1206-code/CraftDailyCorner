namespace CraftDailyCorner.Areas.Admin.ViewModels.CreatorReview
{
    public class VMAdminCreatorReviewDetail
    {
        public int ApplicationID { get; set; }

        public string MemberID { get; set; } = null!;
        public string MemberName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public string BrandName { get; set; } = null!;
        public string? BrandIntro { get; set; }
        public string? PortfolioUrl { get; set; }
        public string? BankCode { get; set; }
        public string? BankAccount { get; set; }
        public DateTime StartDate { get; set; }


        public DateTime AppliedAt { get; set; }
        public string ReviewerName { get; set; } = null!;
        public DateTime? ReviewedAt { get; set; }
       

        public byte StatusID { get; set; }
        public string StatusName { get; set; } = null!;
        public string? ReviewNote { get; set; }
    }
}