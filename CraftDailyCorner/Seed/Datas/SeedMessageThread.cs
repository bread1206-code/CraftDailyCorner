using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedMessageThread
    {
        private readonly CraftDailyCornerContext _context;

        public SeedMessageThread(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.MessageThreads.Any()) // 避免重複 Seed
            {
                var messageThreads = new List<MessageThread>
                {
                    new MessageThread
                    {
                        CreatedAt = DateTime.Now,
                        LastMessageAt = DateTime.Now,
                        LastMessagePreview = "可以的，請在備註說明想刻的內容。",
                        MemberID = "M0000002",
                        CreatorID = "C00001",
                        ProductID = "P000000001"
                    }
                };
                _context.MessageThreads.AddRange(messageThreads);
                _context.SaveChanges();
            }
        }
    }
}
