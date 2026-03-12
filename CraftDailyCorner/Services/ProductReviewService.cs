using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Member;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly CraftDailyCornerContext _context;

        public ProductReviewService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public (bool Success, string Message) UpsertReview(string memberId, VMProductReviewForm vm)
        {
            try
            {
                var order = _context.Orders
                    .AsNoTracking()
                    .FirstOrDefault(o => o.OrderID == vm.OrderID && o.MemberID == memberId);

                if (order == null)
                    return (false, "找不到該訂單");

                if (order.StatusID != 5)
                    return (false, "僅限已完成訂單可評價商品");

                var orderDetailExists = _context.OrderDetails.Any(od =>
                    od.OrderID == vm.OrderID &&
                    od.ProductID == vm.ProductID);

                if (!orderDetailExists)
                    return (false, "該商品不在此訂單中");

                var review = _context.ProductReviews
                    .FirstOrDefault(r =>
                        r.MemberID == memberId &&
                        r.OrderID == vm.OrderID &&
                        r.ProductID == vm.ProductID);

                if (review == null)
                {
                    review = new ProductReview
                    {
                        MemberID = memberId,
                        OrderID = vm.OrderID,
                        ProductID = vm.ProductID,
                        Rating = vm.Rating,
                        Comment = string.IsNullOrWhiteSpace(vm.Comment) ? null : vm.Comment.Trim(),
                        CreatedAt = DateTime.Now
                    };

                    _context.ProductReviews.Add(review);
                }
                else
                {
                    review.Rating = vm.Rating;
                    review.Comment = string.IsNullOrWhiteSpace(vm.Comment) ? null : vm.Comment.Trim();
                    review.UpdatedAt = DateTime.Now;
                }

                _context.SaveChanges();
                return (true, "商品評價已儲存");
            }
            catch (DbUpdateException)
            {
                return (false, "評價儲存失敗，可能已存在重複資料");
            }
            catch
            {
                return (false, "系統錯誤，無法儲存評價");
            }
        }
    }
}