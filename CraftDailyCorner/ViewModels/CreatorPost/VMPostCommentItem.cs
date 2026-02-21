using System;
using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorPost
{
    public class VMPostCommentItem
    {
        public string CommentID { get; set; } = null!;

        public string PostID { get; set; } = null!;

        public string MemberID { get; set; } = null!;

        [Display(Name = "留言者")]
        public string MemberName { get; set; } = null!;

        //會員頭像
        public string? MemberAvatar { get; set; }

        [Display(Name = "留言內容")]
        public string Content { get; set; } = null!;

        [Display(Name = "留言時間")]
        public DateTime CreatedAt { get; set; }

        //留言狀態
        public PostCommentStatus Status { get; set; }



        public string AvatarPath =>
            string.IsNullOrEmpty(MemberAvatar)
                ? "/images/default-avatar.png"
                : $"/Photos/01Member/Thumbnail/{MemberAvatar}.png";


        //是否為留言者本人
        public bool IsOwner { get; set; }


    }
}