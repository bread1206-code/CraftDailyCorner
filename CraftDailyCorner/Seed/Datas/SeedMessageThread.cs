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
            if (!_context.MessageThread.Any()) // 避免重複 Seed
            {
                var messageThreads = new List<MessageThread>
                {
                    new MessageThread
                    {
                        CreatedAt = DateTime.Now,
                        MemberID = "M0000002",
                        CreatorID = "M0000004"
                    }
                };
                _context.MessageThread.AddRange(messageThreads);
                _context.SaveChanges();
            }
        }
    }
}
