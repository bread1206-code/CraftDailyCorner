using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedProductImage
    {
        private readonly CraftDailyCornerContext _context;

        public SeedProductImage(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run(string[] imageGuids)
        {
            if (!_context.ProductImages.Any()) // 避免重複 Seed
            {
                var productImages = new List<ProductImage>
                {
                    new ProductImage
                        {
                            ImageUrl = imageGuids[0] + ".png",
                            SortOrder = 0,
                            StatusID = 1,
                            ProductID = "P000000001"
                        },
                        new ProductImage
                        {
                            ImageUrl = imageGuids[1] + ".png",
                            SortOrder = 1,
                            StatusID = 1,
                            ProductID = "P000000001"
                        },
                        new ProductImage
                        {
                            ImageUrl = imageGuids[2] + ".png",
                            SortOrder = 0,
                            StatusID = 1,
                            ProductID = "P000000002"
                        },
                        new ProductImage
                        {
                            ImageUrl = imageGuids[3] + ".png",
                            SortOrder = 0,
                            StatusID = 1,
                            ProductID = "P000000003"
                        },
                        new ProductImage
                        {
                            ImageUrl = imageGuids[4] + ".png",
                            SortOrder = 0,
                            StatusID = 1,
                            ProductID = "P000000004"
                        },
                        new ProductImage
                        {
                            ImageUrl = imageGuids[5] + ".png",
                            SortOrder = 0,
                            StatusID = 1,
                            ProductID = "P000000005"
                        }
                };
                _context.ProductImages.AddRange(productImages);
                _context.SaveChanges();
            }
        }
    }
}
