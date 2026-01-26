using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class Message
    {
        [Key]
        [Display(Name = "訊息編號")]
        public long MessageID { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [Required(ErrorMessage = "必填欄位")]
        [Display(Name = "內容")]
        public string Content { get; set; }= null!;

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "連線編號")]
        public int ThreadID { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "寄件人")]
        public string SenderID { get; set; } = null!;

        public virtual MessageThread MessageThread { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
    }
}
