using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Member;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers
{
    [Authorize]
    public class ProductReviewsController : Controller
    {
        private readonly IProductReviewService _productReviewService;

        public ProductReviewsController(IProductReviewService productReviewService)
        {
            _productReviewService = productReviewService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(VMProductReviewForm vm)
        {
            var memberId = User.GetMemberId();

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "評價資料格式不正確";
                return RedirectToAction("Detail", "Orders", new { orderId = vm.OrderID });
            }

            var result = _productReviewService.UpsertReview(memberId!, vm);
            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction("Detail", "Orders", new { orderId = vm.OrderID });
        }
    }
}