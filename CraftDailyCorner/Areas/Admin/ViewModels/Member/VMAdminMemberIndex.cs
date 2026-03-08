using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Areas.Admin.ViewModels.Member
{
    public class VMAdminMemberIndex
    {
        // risk / all / admin
        public string Mode { get; set; } = "all";
        // 搜尋用（MemberID）
        public string? SearchMemberId { get; set; }
        // 分頁資訊
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 8;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public List<VMAdminMemberListItem> Items { get; set; } = new();
    }

    public class VMAdminMemberListItem
    {
        [Display(Name = "會員編號")]
        public string MemberID { get; set; } = null!;
        [Display(Name = "暱稱")]
        public string DisplayName { get; set; } = null!;
        [Display(Name = "狀態")]
        public byte StatusID { get; set; }
        public string StatusName { get; set; } = null!;
        public int ViolationCount { get; set; }
        [Display(Name = "註冊時間")]
        public DateTime CreatedAt { get; set; }
        public string? CreatorID { get; set; }
        public string? BrandName { get; set; }
        public List<string> RoleIDs { get; set; } = new();
    }
}
