using CraftDailyCorner.Models.Enums;

namespace CraftDailyCorner.Services
{

    public class ReportResponse
    {
        public ReportResponseEnum Result { get; set; }
        public string? TargetID { get; set; }
    }
}

