using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.ViewModels.Report
{
    public class VMReport
    {
        //類型
        public ReportTargetType ReportType { get; set; }
        //目標ID
        public string TargetID { get; set; } = null!;
    }
}
