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

        [StringLength(40, MinimumLength = 11)]
        [Display(Name ="頭像")]
        public string ImageUrl { get; set; } = "default.png";

        [StringLength(20,MinimumLength =1)]
        [Required]
        [Display(Name ="暱稱")]
        public string DisplayName { get; set; } = null!;

        [HiddenInput]
        [Display(Name ="狀態")]
        public byte StatusID { get; set; }

        [HiddenInput]
        [Display(Name ="建立時間")]
        public DateTime CreatedAt { get; set; }= DateTime.Now;

        public virtual Privacy? Privacy { get; set; }
        public virtual List<MemberRole>? MemberRoles { get; set; }
        public virtual List<CreatorApplication>? CreatorApplications { get; set; }
        public virtual List<CreatorApplication>? ReviewedCreatorApplications { get; set; }
        public virtual CreatorProfile? CreatorProfile { get; set; }= null!;
        public virtual List<Cart>? Carts { get; set; }
        public virtual List<Order>? Orders { get; set; }
        public virtual List<ProductReview>? ProductReviews { get; set; }
        public virtual List<FavoriteProduct>? FavoriteProducts { get; set; }
        public virtual List<FollowCreator>? FollowCreators { get; set; }
        public virtual List<MessageThread>? MessageThreads { get; set; }
        public virtual List<PlatformAnnouncement>? PlatformAnnouncements { get; set; }
        public virtual List<HomepageBanner>? HomepageBanners { get; set; }
        public virtual List<PlatformSetting>? PlatformSettings { get; set; }
        public virtual List<NotificationPreference>? NotificationPreferences { get; set; }
        public virtual List<NotificationEvent>? NotificationEvents { get; set; }
        public virtual List<PostComment>? PostComments { get; set; }
        public virtual List<Message>? Messages { get; set; }
        public virtual List<PasswordResetToken>? PasswordResetTokens { get; set; }
        public virtual MemberStatus? MemberStatus { get; set; }
        public virtual List<PostCommentReport>? PostCommentReports { get; set; }
        public virtual List<MemberRoleHistory> RoleChangeHistories { get; set; } = null!;
        public virtual List<MemberRoleHistory> OperatedRoleHistories { get; set; } = null!;
    }
}
