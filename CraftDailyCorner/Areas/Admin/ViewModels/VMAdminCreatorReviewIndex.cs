namespace CraftDailyCorner.Areas.Admin.ViewModels.CreatorReview
{
    public class VMAdminCreatorReviewIndex
    {
        public List<VMAdminCreatorReviewListItem> Items { get; set; } = new();
    }

    public class VMAdminCreatorReviewListItem
    {
        public int ApplicationID { get; set; }

        public string MemberID { get; set; } = null!;
        public string MemberName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public string DisplayName { get; set; } = null!;
        public DateTime AppliedAt { get; set; }

        public byte StatusID { get; set; }
        public string StatusName { get; set; } = null!;
    }
}