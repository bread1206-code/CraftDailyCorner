using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class Product
    {
        [Key]
        [Display(Name = "商品編號")]
        [StringLength(10,MinimumLength =10)]
        [Column(TypeName = "nchar(10)")]
        public string ProductID { get; set; } = null!;
        [Display(Name = "商品名稱")]
        [StringLength(40,MinimumLength =1)]
        [Required(ErrorMessage ="必填欄位")]
        public string ProductName { get; set; } = null!;
        [Display(Name = "商品描述")]
        [Required(ErrorMessage = "必填欄位")]
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = null!;
        [Column(TypeName = "money")]
        [Display(Name = "商品價格")]
        [Required(ErrorMessage = "必填欄位")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal Price { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "商品成本")]
        [Required(ErrorMessage = "必填欄位")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal CostPrice { get; set; }

        [Display(Name = "狀態")]
        public byte StatusID { get; set; }
        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "所屬創作者")]
        [StringLength(6,MinimumLength =6)]
        public string CreatorID { get; set; } = null!;

        // 導覽屬性
        public virtual CreatorProfile CreatorProfile { get; set; } = null!;
        public virtual List<ProductImage> ProductImages { get; set; }   = new();
        public virtual List<ProductCategory> ProductCategories { get; set; } = new();
        public virtual List<ProductTag> ProductTags { get; set; } = new();
        public virtual List<OrderDetail> OrderDetails { get; set; } = new();
        public virtual List<CartItem> CartItems { get; set; } = new();
        public virtual Inventory Inventory { get; set; } = null!;
        public virtual List<FavoriteProduct> FavoriteProducts { get; set; } = new();
        public virtual List<ProductReview> ProductReviews { get; set; } = new();
        public virtual ProductStatus ProductStatus { get; set; } = null!;
        public virtual List<MessageThread>? MessageThreads { get; set; }
    }
}
