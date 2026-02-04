using CraftDailyCorner.Models;

namespace CraftDailyCorner.ViewModels.Front
{
    public class VMProductDetail
    {
        public Product Product { get; set; }= null!;
        public List<ProductImage> Images { get; set; }= null!;
        public CreatorProfile Creator { get; set; }= null!;
        public List<Tag> Tags { get; set; }= null!;
        public List<Category> Categories { get; set; }= null!;
        public bool IsFavorite { get; set; }
        public string DisplayPrice => Product.Price.ToString("N0");
    }
}
