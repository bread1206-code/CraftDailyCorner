using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.Services
{

    public class ReportResponse
    {
        public ReportResponseEnum Result { get; set; }
        public string? TargetID { get; set; }
    }
}

