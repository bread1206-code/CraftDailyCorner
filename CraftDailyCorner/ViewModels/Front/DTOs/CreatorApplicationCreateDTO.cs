namespace CraftDailyCorner.ViewModels.Front.DTOs
{
    public class CreatorApplicationCreateDTO
    {
        public string MemberId { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Intro { get; set; } = null!;
        public string PortfolioSampleUrl { get; set; } = null!;
        public DateTime StartDate { get; set; }
    }
}
