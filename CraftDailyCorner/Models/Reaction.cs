using CraftDailyCorner.Models.enums;
using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Models
{
    public class Reaction
    {
        [Key]
        [Display(Name = "反應編號")]
        public long ReactionID { get; set; }
        
        [Display(Name = "目標類型")]    
        public ReactionTargetType TargetType { get; set; }
        
        [Display(Name = "目標編號")]
        [StringLength(36)]
        public string TargetID { get; set; } = null!;

        [Display(Name = "反應類型")]
        public ReactionType  ReactionType { get; set; }
        
        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "會員編號")]
        public string MemberID { get; set; } = null!;

        public virtual Member Member { get; set; } = null!;

    }
}
