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
        public MemberStatus Status { get; set; } = 0;

        [HiddenInput]
        [Display(Name ="建立時間")]
        public DateTime CreatedAt { get; set; }= DateTime.Now;

        public virtual Privacy? Privacy { get; set; }
        public virtual List<MemberRole>? MemberRole { get; set; }
        public virtual List<MemberRoleHistory>? MemberRoleHistory { get; set; }
        public virtual List<CreatorApplication>? CreatorApplication { get; set; }
        public virtual CreatorProfile? CreatorProfile { get; set; }= null!;
        public virtual List<Cart>? Cart { get; set; }
        public virtual List<Order>? Order { get; set; }
        public virtual List<ProductReview>? ProductReview { get; set; }
        public virtual List<FavoriteProduct>? FavoriteProduct { get; set; }
        public virtual List<FollowCreator>? FollowCreator { get; set; }
        public virtual List<MessageThread> MessageThread { get; set; } = null!;
        public virtual List<PlatformAnnouncement>? PlatformAnnouncement { get; set; }
        public virtual List<HomepageBanner>? HomepageBanner { get; set; }
        public virtual List<PlatformSetting>? PlatformSetting { get; set; }
        public virtual List<NotificationPreference>? NotificationPreference { get; set; }
        public virtual List<NotificationEvent>? NotificationEvent { get; set; }
        public virtual List<PostComment>? PostComment { get; set; }
        public virtual List<Message>? Message { get; set; }

    }
}
