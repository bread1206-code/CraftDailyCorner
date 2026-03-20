using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Areas.Admin.ViewModels.Member
{
    public class VMAdminAssignGeneralAdmin
    {
        [Display(Name = "手機")]
        public string? SearchPhone { get; set; }

        public bool HasSearched { get; set; }
        public bool IsFound { get; set; }
        public string? SearchMessage { get; set; }

        [Display(Name = "會員編號")]
        public string? MemberID { get; set; }

        [Display(Name = "暱稱")]
        public string? DisplayName { get; set; }

        public byte StatusID { get; set; }

        [Display(Name = "帳號狀態")]
        public string? StatusName { get; set; }

        [Display(Name = "電子郵件")]
        public string? Email { get; set; }

        [Display(Name = "電話")]
        public string? Phone { get; set; }

        public List<string> RoleIDs { get; set; } = new();
        public List<string> RoleNames { get; set; } = new();

        public bool CanAssign { get; set; }
        public string? BlockReason { get; set; }
    }
}