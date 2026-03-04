namespace CraftDailyCorner.DTOs
{
    public class CreatorApplicationCreateDTO
    {
        public string MemberId { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public string BrandIntro { get; set; } = null!;
        public string PortfolioSampleUrl { get; set; } = null!;
        public DateTime StartDate { get; set; }
    }
}
