using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedProductReviews
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedProductReviews(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.ProductReviews == null || !seedContext.ProductReviews.Any())
                throw new Exception("DemoSeedContext.ProductReviews 沒有資料");

            var existingReviewKeys = _context.ProductReviews
                .Select(x => new { x.MemberID, x.OrderID, x.ProductID })
                .ToList()
                .Select(x => $"{x.MemberID}|{x.OrderID}|{x.ProductID}")
                .ToHashSet();

            var existingMemberIds = _context.Members
                .Select(x => x.MemberID)
                .ToHashSet();

            var existingOrders = _context.Orders
                .Select(x => new { x.OrderID, x.MemberID, x.StatusID })
                .ToDictionary(x => x.OrderID, x => new { x.MemberID, x.StatusID });

            var existingProductIds = _context.Products
                .Select(x => x.ProductID)
                .ToHashSet();

            var existingOrderDetailKeys = _context.OrderDetails
                .Select(x => new { x.OrderID, x.ProductID })
                .ToList()
                .Select(x => $"{x.OrderID}|{x.ProductID}")
                .ToHashSet();

            var reviews = new List<ProductReview>();

            int skippedMissingOrder = 0;
            int skippedNotCompleted = 0;
            int skippedMissingProduct = 0;
            int skippedMissingOrderDetail = 0;
            int skippedInvalidRating = 0;
            int skippedInvalidTime = 0;
            int skippedDuplicate = 0;

            foreach (var row in seedContext.ProductReviews)
            {
                if (!existingOrders.TryGetValue(row.OrderID, out var orderInfo))
                {
                    skippedMissingOrder++;
                    continue;
                }

                var actualMemberId = orderInfo.MemberID;
                var reviewKey = $"{actualMemberId}|{row.OrderID}|{row.ProductID}";

                if (existingReviewKeys.Contains(reviewKey))
                {
                    skippedDuplicate++;
                    continue;
                }

                if (!existingMemberIds.Contains(actualMemberId))
                    continue;

                if (orderInfo.StatusID != 5)
                {
                    skippedNotCompleted++;
                    continue;
                }

                if (!existingProductIds.Contains(row.ProductID))
                {
                    skippedMissingProduct++;
                    continue;
                }

                var orderDetailKey = $"{row.OrderID}|{row.ProductID}";
                if (!existingOrderDetailKeys.Contains(orderDetailKey))
                {
                    skippedMissingOrderDetail++;
                    continue;
                }

                if (row.Rating < 1 || row.Rating > 5)
                {
                    skippedInvalidRating++;
                    continue;
                }

                if (row.UpdatedAt.HasValue && row.UpdatedAt.Value < row.CreatedAt)
                {
                    skippedInvalidTime++;
                    continue;
                }

                reviews.Add(new ProductReview
                {
                    Rating = row.Rating,
                    Comment = row.Comment,
                    CreatedAt = row.CreatedAt,
                    UpdatedAt = row.UpdatedAt,
                    MemberID = actualMemberId,
                    OrderID = row.OrderID,
                    ProductID = row.ProductID
                });

                existingReviewKeys.Add(reviewKey);
            }

            if (reviews.Any())
            {
                _context.ProductReviews.AddRange(reviews);
                _context.SaveChanges();
            }

            Console.WriteLine("=== DemoSeedProductReviews ===");
            Console.WriteLine($"新增 Reviews：{reviews.Count}");
            Console.WriteLine($"略過 - 找不到訂單：{skippedMissingOrder}");
            Console.WriteLine($"略過 - 非完成訂單：{skippedNotCompleted}");
            Console.WriteLine($"略過 - 找不到商品：{skippedMissingProduct}");
            Console.WriteLine($"略過 - 找不到 OrderDetail：{skippedMissingOrderDetail}");
            Console.WriteLine($"略過 - Rating 不合法：{skippedInvalidRating}");
            Console.WriteLine($"略過 - 時間不合法：{skippedInvalidTime}");
            Console.WriteLine($"略過 - 重複資料：{skippedDuplicate}");
        }
    }
}