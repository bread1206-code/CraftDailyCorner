using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class Payment
    {
        [Key]
        [Display(Name = "支付歷程編號")]
        public int PaymentID { get; set; }

        [Display(Name = "交易方式")]
        public PaymentPaymentMethod PaymentMethod { get; set; }

        [Display(Name = "本次付款金額")]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Display(Name = "狀態")]
        public PaymentStatus Status { get; set; }

        [StringLength(50)]
        [Display(Name = "金流交易編號")]
        public string GatewayTradeNo { get; set; }= null!;

        [Display(Name = "第幾次付款嘗試")]
        public byte AttemptNo { get; set; }

        [Display(Name = "付款完成時間")]
        public DateTime PaidAt { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [StringLength(12, MinimumLength = 12)]
        [Column(TypeName = "nchar(12)")]
        [Display(Name = "訂單編號")]
        public string OrderID { get; set; }= null!;

        public virtual Order Order { get; set; }= null!;
    }
}
