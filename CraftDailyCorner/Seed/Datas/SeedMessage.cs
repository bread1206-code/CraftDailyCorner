using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedMessage
    {
        private readonly CraftDailyCornerContext _context;

        public SeedMessage(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.Messages.Any()) // 避免重複 Seed
            {
                var messages = new List<Message>
                {
                    new Message
                    {
                        Content = "請問這個商品可以客製刻字嗎？",
                        CreatedAt = DateTime.Now,
                        ThreadID = 1,
                        SenderID = "M0000002",
                        IsRead = true
                    },
                    new Message
                    {
                        Content = "可以的，請在備註說明想刻的內容。",
                        CreatedAt = DateTime.Now,
                        ThreadID = 1,
                        SenderID = "M0000004",
                        IsRead = false
                    }
                };
                _context.Messages.AddRange(messages);
                _context.SaveChanges();
            }
        }
    }
}
