using CraftDailyCorner.Models;
using CraftDailyCorner.Models.Enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Message;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class MessageTemplateService : IMessageTemplateService
    {
        private readonly CraftDailyCornerContext _context;

        public MessageTemplateService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<VMMessageTemplateManage> GetManageVmAsync(string creatorId)
        {
            var items = await _context.AutoReplyTemplates
                .AsNoTracking()
                .Where(t => t.CreatorID == creatorId)
                // 自動回覆模板固定排最上面，其餘快速回覆排下面
                .OrderBy(t => t.TriggerType == AutoReplyTemplateTriggerType.FirstMessage ? 0 : 1)
                .ThenBy(t => t.CreatedAt)
                .ThenBy(t => t.TemplateID)
                .Select(t => new VMMessageTemplateManageItem
                {
                    TemplateID = t.TemplateID,
                    Title = t.Title,
                    Content = t.Content,
                    TriggerType = (byte)t.TriggerType,
                    TriggerTypeName = t.TriggerType == AutoReplyTemplateTriggerType.FirstMessage
                        ? "第一次訊息自動回覆"
                        : "快速回覆模板",
                    IsActive = t.IsActive
                })
                .ToListAsync();

            var quickReplyCount = items.Count(x => x.TriggerType == (byte)AutoReplyTemplateTriggerType.QuickReply);

            return new VMMessageTemplateManage
            {
                Items = items,
                QuickReplyCount = quickReplyCount,
                QuickReplyLimit = 10
            };
        }

        public Task<VMMessageTemplateUpsert> GetCreateVmAsync()
        {
            // 建立頁只給快速回覆模板使用
            return Task.FromResult(new VMMessageTemplateUpsert
            {
                IsActive = true,
                TriggerType = AutoReplyTemplateTriggerType.QuickReply
            });
        }

        public async Task<VMMessageTemplateUpsert?> GetEditVmAsync(int id, string creatorId)
        {
            return await _context.AutoReplyTemplates
                .AsNoTracking()
                .Where(t => t.TemplateID == id && t.CreatorID == creatorId)
                .Select(t => new VMMessageTemplateUpsert
                {
                    TemplateID = t.TemplateID,
                    Title = t.Title,
                    Content = t.Content,
                    TriggerType = t.TriggerType,
                    IsActive = t.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(VMMessageTemplateUpsert vm, string creatorId)
        {
            // 規則：只能新增快速回覆模板
            if (vm.TriggerType != AutoReplyTemplateTriggerType.QuickReply)
                throw new ArgumentException("自動回覆模板不可新增，只能編輯或啟用/禁用既有模板。");

            // 規則：快速回覆模板上限 10 個
            var quickReplyCount = await _context.AutoReplyTemplates
                .CountAsync(t => t.CreatorID == creatorId
                              && t.TriggerType == AutoReplyTemplateTriggerType.QuickReply);

            if (quickReplyCount >= 10)
                throw new ArgumentException("快速回覆模板最多只能建立 10 個。");

            var title = (vm.Title ?? string.Empty).Trim();
            var content = (vm.Content ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("請輸入模板標題");

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("請輸入模板內容");

            var entity = new AutoReplyTemplate
            {
                Title = title,
                Content = content,
                TriggerType = AutoReplyTemplateTriggerType.QuickReply,
                IsActive = vm.IsActive,
                CreatorID = creatorId,
                CreatedAt = DateTime.Now
            };

            _context.AutoReplyTemplates.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(VMMessageTemplateUpsert vm, string creatorId)
        {
            if (vm.TemplateID == null)
                return false;

            var entity = await _context.AutoReplyTemplates
                .FirstOrDefaultAsync(t => t.TemplateID == vm.TemplateID && t.CreatorID == creatorId);

            if (entity == null)
                return false;

            var title = (vm.Title ?? string.Empty).Trim();
            var content = (vm.Content ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("請輸入模板標題");

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("請輸入模板內容");

            // 只能修改內容 / 標題 / 啟用狀態
            // 不允許從畫面把 TriggerType 改掉
            entity.Title = title;
            entity.Content = content;
            entity.IsActive = vm.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EnableAsync(int id, string creatorId)
        {
            var entity = await _context.AutoReplyTemplates
                .FirstOrDefaultAsync(t => t.TemplateID == id && t.CreatorID == creatorId);

            if (entity == null)
                return false;

            if (entity.IsActive)
                return true;

            entity.IsActive = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DisableAsync(int id, string creatorId)
        {
            var entity = await _context.AutoReplyTemplates
                .FirstOrDefaultAsync(t => t.TemplateID == id && t.CreatorID == creatorId);

            if (entity == null)
                return false;

            if (!entity.IsActive)
                return true;

            entity.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}