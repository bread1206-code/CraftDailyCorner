using CraftDailyCorner.Services;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CraftDailyCorner.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        

    [Authorize]
    public IActionResult Index()
    {
        var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(memberId))
            return Unauthorized();

        var payments = _paymentService.GetMyPayments(memberId);

        return View(payments);
    }
    // 顯示付款頁
    [HttpGet]
        public IActionResult Create(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                return BadRequest();

            var vm = _paymentService.PreparePayment(orderId);

            if (vm == null)
                return NotFound();

            if (vm.IsPaid)
            {
                return RedirectToAction(nameof(Result), new { orderId });
            }
            return View(vm);
        }
        // 送出付款（Mock）
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VMPaymentSubmit vm)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var result = _paymentService.CreateMockPayment(vm);

                // 導向結果頁（避免重新整理重複付款）
                return RedirectToAction(nameof(Result), new { orderId = result.OrderID });
            }
            catch (Exception ex)
            {
                // 這裡可換成 Logger
                TempData["PaymentError"] = ex.Message;
                return RedirectToAction(nameof(Create), new { orderId = vm.OrderID });
            }
        }
        // 付款結果頁
        [HttpGet]
        public IActionResult Result(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                return BadRequest();

            // 這裡可以重用訂單詳情 or Payment 紀錄
            var payments = _paymentService.GetPaymentsByOrder(orderId);

            if (payments == null || !payments.Any())
                return NotFound();

            var latestPayment = payments
                .OrderByDescending(p => p.AttemptNo)
                .First();

            var vm = new VMPaymentResult
            {
                OrderID = orderId,
                Amount = latestPayment.Amount,
                PaymentStatusID = latestPayment.StatusID,
                PaymentStatusName = latestPayment.StatusName,
                PaidAt = latestPayment.PaidAt,
                Message = latestPayment.StatusName == "成功"
                    ? "付款完成，感謝您的訂購！"
                    : "付款失敗，請重新嘗試"
            };

            return View(vm);
        }
    }
}