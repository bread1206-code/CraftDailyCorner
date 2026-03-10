using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedAutoReplyTemplate
    {
        private readonly CraftDailyCornerContext _context;

        public SeedAutoReplyTemplate(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.AutoReplyTemplates.Any()) // 避免重複 Seed
            {
                var autoReplyTemplates = new List<AutoReplyTemplate>
                {
                    // C00001 固定唯一 FirstMessage
                    new AutoReplyTemplate
                    {
                        Title = "首次訊息自動回覆",
                        Content = "您好，感謝您的來訊！我會盡快回覆您，謝謝。",
                        IsActive = true,
                        TriggerType = AutoReplyTemplateTriggerType.FirstMessage,
                        CreatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    },

                    // C00001 QuickReply
                    new AutoReplyTemplate
                    {
                        Title = "感謝詢問",
                        Content = "您好，感謝您的詢問，我會盡快為您確認並回覆。",
                        IsActive = true,
                        TriggerType = AutoReplyTemplateTriggerType.QuickReply,
                        CreatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    },
                    new AutoReplyTemplate
                    {
                        Title = "確認庫存中",
                        Content = "您好，這邊幫您確認庫存中，請稍候一下，謝謝。",
                        IsActive = true,
                        TriggerType = AutoReplyTemplateTriggerType.QuickReply,
                        CreatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    },
                    new AutoReplyTemplate
                    {
                        Title = "客製需求",
                        Content = "您好，若有客製需求，歡迎提供想法與細節，我會再與您討論。",
                        IsActive = true,
                        TriggerType = AutoReplyTemplateTriggerType.QuickReply,
                        CreatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    },

                    // C00002 固定唯一 FirstMessage
                    new AutoReplyTemplate
                    {
                        Title = "首次訊息自動回覆",
                        Content = "您好，感謝您的來訊！我會盡快與您聯繫。",
                        IsActive = true,
                        TriggerType = AutoReplyTemplateTriggerType.FirstMessage,
                        CreatedAt = DateTime.Now,
                        CreatorID = "C00002"
                    },

                    // C00002 QuickReply
                    new AutoReplyTemplate
                    {
                        Title = "感謝詢問",
                        Content = "您好，感謝您的詢問，我會盡快為您確認並回覆。",
                        IsActive = true,
                        TriggerType = AutoReplyTemplateTriggerType.QuickReply,
                        CreatedAt = DateTime.Now,
                        CreatorID = "C00002"
                    },
                    new AutoReplyTemplate
                    {
                        Title = "確認庫存中",
                        Content = "您好，這邊幫您確認庫存中，請稍候一下，謝謝。",
                        IsActive = true,
                        TriggerType = AutoReplyTemplateTriggerType.QuickReply,
                        CreatedAt = DateTime.Now,
                        CreatorID = "C00002"
                    },
                    new AutoReplyTemplate
                    {
                        Title = "客製需求",
                        Content = "您好，若有客製需求，歡迎提供想法與細節，我會再與您討論。",
                        IsActive = true,
                        TriggerType = AutoReplyTemplateTriggerType.QuickReply,
                        CreatedAt = DateTime.Now,
                        CreatorID = "C00002"
                    }
                };
                _context.AutoReplyTemplates.AddRange(autoReplyTemplates);
                _context.SaveChanges();
            }
        }
    }
}
