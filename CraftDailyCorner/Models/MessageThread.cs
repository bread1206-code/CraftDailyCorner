using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class MessageThread
    {
        [Key]
        [Display(Name = "連線編號")]
        public int ThreadID { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "會員編號")]
        public string MemberID { get; set; } = null!;

        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "會員編號")]
        public string CreatorID { get; set; } = null!;

        public virtual Member Member { get; set; } = null!;
        //public virtual CreatorProfile CreatorProfile { get; set; } = null!;
        public virtual List<Message>? Message { get; set; }
    }
}
