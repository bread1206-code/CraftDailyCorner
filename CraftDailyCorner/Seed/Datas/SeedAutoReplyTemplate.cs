using CraftDailyCorner.Models;

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
            if (!_context.AutoReplyTemplate.Any()) // 避免重複 Seed
            {
                var autoReplyTemplates = new List<AutoReplyTemplate>
                {
                    new AutoReplyTemplate
                    {
                        Title = "客製詢問回覆",
                        Content = "您好，客製需求請提供詳細說明，謝謝。",
                        IsActive = true,
                        TriggerType = (AutoReplyTemplateTriggerType)1,
                        CreatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    },
                    new AutoReplyTemplate
                    {
                        Title = "第一次建立對話自動回覆",
                        Content = "您好，目前正在創作當中，還請您稍等，謝謝。",
                        IsActive = true,
                        TriggerType = (AutoReplyTemplateTriggerType)1,
                        CreatedAt = DateTime.Now,
                        CreatorID = "C00002"
                    }
                };
                _context.AutoReplyTemplate.AddRange(autoReplyTemplates);
                _context.SaveChanges();
            }
        }
    }
}
