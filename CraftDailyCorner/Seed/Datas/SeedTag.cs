using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedTag
    {
        private readonly CraftDailyCornerContext _context;

        public SeedTag(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.Tags.Any()) // 避免重複 Seed
            {
                var tags = new List<Tag>
                {
                    new Tag { TagName = "手工",//1
                        IsActive = true,
                        CreatedAt = DateTime.Now 
                    },
                    new Tag { TagName = "限量" ,//2
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new Tag { TagName = "原創" ,//3
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new Tag { TagName = "客製化" ,//4
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new Tag { TagName = "手作課程體驗" ,//5
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new Tag { TagName = "親子活動套組" ,//6
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    },
                    new Tag { TagName = "原木" ,//7
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    }
                };
                _context.Tags.AddRange(tags);
                _context.SaveChanges();
            }
        }
    }
}
