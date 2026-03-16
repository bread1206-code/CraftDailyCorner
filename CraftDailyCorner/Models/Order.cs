using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class Order
    {
        [Key]
        [StringLength(12,MinimumLength =12)]
        [Column(TypeName = "nchar(12)")]
        [Display(Name = "訂單編號")]
        public string OrderID { get; set; } = null!;

        [StringLength(20)]
        [Required(ErrorMessage = "必填欄位")]
        [Display(Name = "收件人姓名")]
        public string ReceiverName { get; set; } = null!;

        [RegularExpression("[0][9][0-9]{8}", ErrorMessage = "手機格式錯誤")]
        [StringLength(10,MinimumLength =10)]
        [Column(TypeName = "nchar(10)")]
        [Required(ErrorMessage = "必填欄位")]
        [Display(Name = "收件人電話")]
        public string ReceiverPhone { get; set; } = null!;
        
        [StringLength(50)]
        [Required(ErrorMessage = "必填欄位")]
        [Display(Name = "收件人地址")]
        public string ShippingAddress { get; set; } = null!;

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "狀態")]
        public byte StatusID { get; set; }

        [Display(Name = "總金額")]
        [Column(TypeName = "money")]
        public decimal TotalAmount { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "會員編號")]
        public string MemberID { get; set; } = null!;

        public virtual Member Member { get; set; } = null!;
        public virtual List<OrderDetail> OrderDetails { get; set; } = new();
        public virtual List<Payment> Payments { get; set; } = new();
        public virtual Shipment? Shipment { get; set; }
        public virtual OrderStatus OrderStatus { get; set; } = null!;
        public virtual List<ProductReview> ProductReviews { get; set; } = new();
    }
}
