using CraftDailyCorner.ViewModels.Reaction;

namespace CraftDailyCorner.ViewModels.CreatorPortfolio.Front
{
    public class VMPortfolioDetail
    {
        public string PortfolioID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string CreatorName { get; set; } = null!;
        public string CreatorID { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsOwner { get; set; }

        public List<VMPortfolioDetailItem> Items { get; set; } = new();

        public VMReactionButton? ReactionButton { get; set; }

        public bool IsReportBanned { get; set; }
        public DateTime? ReportBanUntil { get; set; }
    }
}
