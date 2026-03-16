using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Seed.Demo.Context;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedAutoReplyTemplates
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedAutoReplyTemplates(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            var creators = _context.CreatorProfiles
                .Select(x => new
                {
                    x.CreatorID,
                    x.CreatedAt
                })
                .ToList();

            if (!creators.Any())
                return;

            var existingTemplateKeys = _context.AutoReplyTemplates
                .Select(x => new
                {
                    x.CreatorID,
                    x.Title,
                    x.TriggerType
                })
                .ToList()
                .Select(x => $"{x.CreatorID}|||{x.Title}|||{(byte)x.TriggerType}")
                .ToHashSet();

            var templates = new List<AutoReplyTemplate>();

            foreach (var creator in creators)
            {
                var now = creator.CreatedAt;

                AddIfNotExists(
                    templates,
                    existingTemplateKeys,
                    creator.CreatorID,
                    "首次訊息自動回覆",
                    "您好，感謝您的來訊！我會盡快回覆您，謝謝您的耐心等候。",
                    AutoReplyTemplateTriggerType.FirstMessage,
                    now);

                AddIfNotExists(
                    templates,
                    existingTemplateKeys,
                    creator.CreatorID,
                    "感謝詢問",
                    "您好，感謝您的詢問，我會盡快為您確認並回覆。",
                    AutoReplyTemplateTriggerType.QuickReply,
                    now);

                AddIfNotExists(
                    templates,
                    existingTemplateKeys,
                    creator.CreatorID,
                    "確認庫存中",
                    "您好，這邊幫您確認庫存中，請稍候一下，謝謝。",
                    AutoReplyTemplateTriggerType.QuickReply,
                    now);

                AddIfNotExists(
                    templates,
                    existingTemplateKeys,
                    creator.CreatorID,
                    "客製需求",
                    "您好，若有客製需求，歡迎提供想法與細節，我會再與您討論。",
                    AutoReplyTemplateTriggerType.QuickReply,
                    now);
            }

            if (templates.Any())
            {
                _context.AutoReplyTemplates.AddRange(templates);
                _context.SaveChanges();
            }
        }

        private static void AddIfNotExists(
            List<AutoReplyTemplate> templates,
            HashSet<string> existingTemplateKeys,
            string creatorId,
            string title,
            string content,
            AutoReplyTemplateTriggerType triggerType,
            DateTime createdAt)
        {
            var key = $"{creatorId}|||{title}|||{(byte)triggerType}";

            if (existingTemplateKeys.Contains(key))
                return;

            templates.Add(new AutoReplyTemplate
            {
                Title = title,
                Content = content,
                IsActive = true,
                TriggerType = triggerType,
                CreatedAt = createdAt,
                CreatorID = creatorId
            });

            existingTemplateKeys.Add(key);
        }
    }
}