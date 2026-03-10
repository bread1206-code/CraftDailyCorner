using CraftDailyCorner.Extensions;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IMessageTemplateService _messageTemplateService;

        public MessagesController(IMessageService messageService, IMessageTemplateService quickReplyTemplateService)
        {
            _messageService = messageService;
            _messageTemplateService = quickReplyTemplateService;
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

        // 送出訊息
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

        // =============================
        // 訊息模板管理
        // =============================

        [HttpGet]
        public async Task<IActionResult> MessageTemplates()
        {
            var creatorId = User.GetCreatorId();

            if (string.IsNullOrWhiteSpace(creatorId))
                return Forbid();

            var vm = await _messageTemplateService.GetManageVmAsync(creatorId);
            return View(vm);
        }

        // =============================
        // 新增快速回覆模板
        // 規則：只能新增 QuickReply，不能新增 FirstMessage
        // =============================

        [HttpGet]
        public async Task<IActionResult> CreateMessageTemplate()
        {
            var creatorId = User.GetCreatorId();

            if (string.IsNullOrWhiteSpace(creatorId))
                return Forbid();

            var vm = await _messageTemplateService.GetCreateVmAsync();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMessageTemplate(VMMessageTemplateUpsert vm)
        {
            var creatorId = User.GetCreatorId();

            if (string.IsNullOrWhiteSpace(creatorId))
                return Forbid();

            // 安全起見：Controller 先固定成 QuickReply
            vm.TriggerType = AutoReplyTemplateTriggerType.QuickReply;

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                await _messageTemplateService.CreateAsync(vm, creatorId);
                TempData["Success"] = "快速回覆模板已新增";
                return RedirectToAction(nameof(MessageTemplates));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        // =============================
        // 編輯模板
        // 規則：
        // - FirstMessage：只能編輯 / 啟用禁用
        // - QuickReply：可編輯 / 啟用禁用
        // - 不可改 TriggerType
        // =============================

        [HttpGet]
        public async Task<IActionResult> EditMessageTemplate(int id)
        {
            var creatorId = User.GetCreatorId();

            if (string.IsNullOrWhiteSpace(creatorId))
                return Forbid();

            var vm = await _messageTemplateService.GetEditVmAsync(id, creatorId);

            if (vm == null)
                return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMessageTemplate(VMMessageTemplateUpsert vm)
        {
            var creatorId = User.GetCreatorId();

            if (string.IsNullOrWhiteSpace(creatorId))
                return Forbid();

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var ok = await _messageTemplateService.UpdateAsync(vm, creatorId);

                if (!ok)
                    return NotFound();

                TempData["Success"] = "訊息模板已更新";
                return RedirectToAction(nameof(MessageTemplates));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableMessageTemplate(int id)
        {
            var creatorId = User.GetCreatorId();

            if (string.IsNullOrWhiteSpace(creatorId))
                return Forbid();

            var ok = await _messageTemplateService.EnableAsync(id, creatorId);

            if (!ok)
                return NotFound();

            TempData["Success"] = "訊息模板已啟用";
            return RedirectToAction(nameof(MessageTemplates));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableMessageTemplate(int id)
        {
            var creatorId = User.GetCreatorId();

            if (string.IsNullOrWhiteSpace(creatorId))
                return Forbid();

            var ok = await _messageTemplateService.DisableAsync(id, creatorId);

            if (!ok)
                return NotFound();

            TempData["Success"] = "訊息模板已禁用";
            return RedirectToAction(nameof(MessageTemplates));
        }
    }
}