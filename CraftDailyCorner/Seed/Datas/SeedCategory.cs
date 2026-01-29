using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedCategory
    {
        private readonly CraftDailyCornerContext _context;

        public SeedCategory(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.Categories.Any()) // 避免重複 Seed
            {
                var categories = new List<Category>
                {
                    new Category { CategoryName = "手作工藝類",
                        IsActive = true,
                        ParentCategoryID= null,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "手作園藝類",
                        IsActive = true,
                        ParentCategoryID= null,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "手作藝術類",
                        IsActive = true,
                        ParentCategoryID= null,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "手作模型類",
                        IsActive = true,
                        ParentCategoryID= null,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "手作香氛保養類",
                        IsActive = true,
                        ParentCategoryID= null,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "手作食品類",
                        IsActive = true,
                        ParentCategoryID= null,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "布作縫紉",
                        IsActive = true,
                        ParentCategoryID = 1,
                        CreatedAt = DateTime.Now 
                    },
                    new Category { CategoryName = "皮革工藝",
                        IsActive = true,
                        ParentCategoryID= 1,
                        CreatedAt = DateTime.Now 
                    },
                    new Category { CategoryName = "木工竹藝",
                        IsActive = true,
                        ParentCategoryID= 1,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "金工飾品",
                        IsActive = true,
                        ParentCategoryID= 1,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "陶藝黏土",
                        IsActive = true,
                        ParentCategoryID= 1,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "紙藝印章",
                        IsActive = true,
                        ParentCategoryID= 1,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "編織纖維",
                        IsActive = true,
                        ParentCategoryID= 1,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "乾燥・永生花",
                        IsActive = true,
                        ParentCategoryID= 2,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "園藝・植栽",
                        IsActive = true,
                        ParentCategoryID= 2,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "居家佈置小物",
                        IsActive = true,
                        ParentCategoryID= 2,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "環保生活手作",
                        IsActive = true,
                        ParentCategoryID= 2,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "插畫・畫作",
                        IsActive = true,
                        ParentCategoryID= 3,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "手繪商品",
                        IsActive = true,
                        ParentCategoryID= 3,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "版畫・拓印",
                        IsActive = true,
                        ParentCategoryID= 3,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "攝影藝術",
                        IsActive = true,
                        ParentCategoryID= 3,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "拼貼藝術",
                        IsActive = true,
                        ParentCategoryID= 3,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "黏土公仔",
                        IsActive = true,
                        ParentCategoryID= 4,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "布娃娃",
                        IsActive = true,
                        ParentCategoryID= 4,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "樹脂模型",
                        IsActive = true,
                        ParentCategoryID= 4,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "積木",
                        IsActive = true,
                        ParentCategoryID= 4,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "香氛",
                        IsActive = true,
                        ParentCategoryID= 5,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "沐浴護理",
                        IsActive = true,
                        ParentCategoryID= 5,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "保養護膚",
                        IsActive = true,
                        ParentCategoryID= 5,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "甜點與烘焙",
                        IsActive = true,
                        ParentCategoryID= 6,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "果醬與醬料",
                        IsActive = true,
                        ParentCategoryID= 6,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "飲品•茶•咖啡",
                        IsActive = true,
                        ParentCategoryID= 6,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "節慶禮盒",
                        IsActive = true,
                        ParentCategoryID= 6,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "健康食品",
                        IsActive = true,
                        ParentCategoryID= 6,
                        CreatedAt = DateTime.Now
                    }
                };
                _context.Categories.AddRange(categories);
                _context.SaveChanges();
            }
        }
    }
}
