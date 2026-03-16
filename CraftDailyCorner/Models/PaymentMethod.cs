using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Models
{
    public class PaymentMethod
    {
        [Key]
        [Display(Name = "支付方式代碼")]
        public byte MethodID { get; set; }
        [Display(Name = "支付方式碼")]
        public string MethodCode { get; set; } = null!;
        [Display(Name = "支付方式名稱")]
        public string MethodName { get; set; } = null!;
        [Display(Name = "描述")]
        public string? Description { get; set; }
        [Display(Name = "是否啟用")]
        public bool IsActive { get; set; }
        public virtual List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
