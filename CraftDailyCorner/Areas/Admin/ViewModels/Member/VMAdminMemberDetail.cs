using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Areas.Admin.ViewModels.Member
{
    public class VMAdminMemberDetail
    {
        [Display(Name = "會員編號")]
        public string MemberID { get; set; } = null!;
        [Display(Name = "暱稱")]
        public string DisplayName { get; set; } = null!;

        public byte StatusID { get; set; }
        [Display(Name = "帳號狀態")]
        public string StatusName { get; set; } = null!;

        [Display(Name = "檢舉次數")]
        public int MaliciousReportCount { get; set; }
        [Display(Name = "檢舉封鎖至")]
        public DateTime? ReportBanUntil { get; set; }
        [Display(Name = "違規次數")]
        public int ViolationCount { get; set; }
        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "電子郵件")]
        public string? Email { get; set; }
        [Display(Name = "電話")]
        public string? Phone { get; set; }

        public List<string> RoleIDs { get; set; } = new();
        public List<string> RoleNames { get; set; } = new();

        // ===== 創作者資料 =====
        [Display(Name = "創作者編號")]
        public string? CreatorID { get; set; }
        public byte? CreatorStatusID { get; set; }
        [Display(Name = "創作者狀態")]
        public string? CreatorStatusName { get; set; }
        [Display(Name = "品牌名稱")]
        public string? BrandName { get; set; }
        [Display(Name = "品牌介紹")]
        public string? BrandIntro { get; set; }
        [Display(Name = "創作起始日")]
        public DateTime? StartDate { get; set; }

        public string? BankCode { get; set; }
        public string? BankAccount { get; set; }
        [Display(Name = "建立時間")]
        public DateTime? CreatorCreatedAt { get; set; }
        [Display(Name = "最後更新時間")]
        public DateTime? UpdatedAt { get; set; }
    }
}