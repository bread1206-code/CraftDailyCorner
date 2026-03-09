using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }


        // 訊息主頁（Inbox + Chat）
        // 會員與創作者都進這一頁

        [HttpGet]
        public async Task<IActionResult> Index(int? threadId = null)
        {
            var memberId = User.GetMemberId();
            var creatorId = User.IsInRole("02") ? User.GetCreatorId() : null;

            var vm = await _messageService.GetInboxAsync(memberId, creatorId, threadId);
            return View(vm);
        }

        // 從商品頁發起詢問
        // 入口：Products/Detail 的「詢問商品」

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartFromProduct(string productId)
        {
            var memberId = User.GetMemberId();

            if (string.IsNullOrWhiteSpace(productId))
                return BadRequest("商品編號不可為空");

            var threadId = await _messageService.GetOrCreateThreadFromProductAsync(memberId, productId);

            return RedirectToAction(nameof(Index), new { threadId });
        }

        //送出訊息
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(int threadId, string content)
        {
            var senderId = User.GetMemberId();

            try
            {
                await _messageService.SendMessageAsync(threadId, senderId, content);
            }
            catch (ArgumentException ex)
            {
                TempData["Warning"] = ex.Message;
            }

            return RedirectToAction(nameof(Index), new { threadId });
        }
    }
}