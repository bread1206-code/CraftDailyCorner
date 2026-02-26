using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class Member
    {
        [Key]
        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name ="會員編號")]
        [HiddenInput]
        public string MemberID { get; set; } = null!;

        [StringLength(36)]
        [Display(Name ="頭像")]
        public string? ImageUrl { get; set; }

        [StringLength(20,MinimumLength =1)]
        [Required]
        [Display(Name ="暱稱")]
        public string DisplayName { get; set; } = null!;

        [HiddenInput]
        [Display(Name ="狀態")]
        public byte StatusID { get; set; }

        [Display(Name = "惡意檢舉次數")]
        public int MaliciousReportCount { get; set; } = 0;

        [Display(Name = "檢舉功能停權至")]
        public DateTime? ReportBanUntil { get; set; }

        [HiddenInput]
        [Display(Name ="建立時間")]
        public DateTime CreatedAt { get; set; }= DateTime.Now;

        public virtual Privacy? Privacy { get; set; }
        public virtual List<MemberRole> MemberRoles { get; set; } = new List<MemberRole>();
        public virtual List<CreatorApplication> CreatorApplications { get; set; } = new List<CreatorApplication>();
        public virtual List<CreatorApplication> ReviewedCreatorApplications { get; set; } = new List<CreatorApplication>();
        public virtual CreatorProfile? CreatorProfile { get; set; }
        public virtual List<Cart> Carts { get; set; } = new List<Cart>();
        public virtual List<Order> Orders { get; set; } = new List<Order>();
        public virtual List<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
        public virtual List<FavoriteProduct> FavoriteProducts { get; set; } = new List<FavoriteProduct>();
        public virtual List<FollowCreator> FollowCreators { get; set; } = new List<FollowCreator>();
        public virtual List<MessageThread> MessageThreads { get; set; } = new List<MessageThread>();
        public virtual List<PlatformAnnouncement> PlatformAnnouncements { get; set; } = new List<PlatformAnnouncement>();
        public virtual List<HomepageBanner> HomepageBanners { get; set; } = new List<HomepageBanner>();
        public virtual List<PlatformSetting> PlatformSettings { get; set; } = new List<PlatformSetting>();
        public virtual List<NotificationPreference> NotificationPreferences { get; set; } = new List<NotificationPreference>();
        public virtual List<NotificationEvent> NotificationEvents { get; set; } = new List<NotificationEvent>();
        public virtual List<PostComment> PostComments { get; set; } = new List<PostComment>();
        public virtual List<Message> Messages { get; set; } = new List<Message>();
        public virtual List<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
        public virtual MemberStatus? MemberStatus { get; set; }
        public virtual List<MemberRoleHistory> RoleChangeHistories { get; set; } = new List<MemberRoleHistory>();
        public virtual List<MemberRoleHistory> OperatedRoleHistories { get; set; } = new List<MemberRoleHistory>();
        public virtual List<Report> ReportsCreated { get; set; } = new List<Report>();
        public virtual List<Report> ReportsReviewed { get; set; } = new List<Report>();
        public virtual List<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}
