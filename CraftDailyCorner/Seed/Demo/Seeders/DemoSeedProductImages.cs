using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;
using CraftDailyCorner.Seed.Demo.Helpers;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedProductImages
    {
        private readonly CraftDailyCornerContext _context;
        private readonly DemoSeedImageHelper _imageHelper;

        public DemoSeedProductImages(
            CraftDailyCornerContext context,
            DemoSeedImageHelper imageHelper)
        {
            _context = context;
            _imageHelper = imageHelper;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.Products == null || !seedContext.Products.Any())
                throw new Exception("DemoSeedContext.Products 沒有資料");

            if (seedContext.ProductImages == null || !seedContext.ProductImages.Any())
                throw new Exception("DemoSeedContext.ProductImages 沒有資料");

            var productMap = seedContext.Products
                .ToDictionary(x => x.ProductID);

            var existingImageKeys = _context.ProductImages
                .Select(x => new { x.ProductID, x.SortOrder })
                .ToHashSet();

            var images = new List<ProductImage>();

            foreach (var row in seedContext.ProductImages)
            {
                if (existingImageKeys.Contains(new { row.ProductID, row.SortOrder }))
                    continue;

                if (!productMap.TryGetValue(row.ProductID, out var product))
                    throw new Exception($"找不到對應商品：{row.ProductID}");

                var sourceFileName = row.SourceImageFileName;

                if (!Path.HasExtension(sourceFileName))
                {
                    sourceFileName += ".png";
                }

                var sourceImagePath = Path.Combine(DemoSeedPaths.ProductRaw, sourceFileName);

                if (!File.Exists(sourceImagePath))
                    throw new FileNotFoundException($"找不到商品圖片：{sourceFileName}", sourceImagePath);

                var imageGuid = _imageHelper.UploadProductImage(
                    sourceFilePath: sourceImagePath,
                    creatorId: product.CreatorID);

                images.Add(new ProductImage
                {
                    ProductID = row.ProductID,
                    ImageUrl = imageGuid,
                    SortOrder = row.SortOrder,
                    StatusID = row.StatusID
                });
            }

            if (images.Any())
            {
                _context.ProductImages.AddRange(images);
                _context.SaveChanges();
            }
        }
    }
}