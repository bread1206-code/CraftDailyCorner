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
                    new Category { CategoryName = "園藝・生活空間",
                        IsActive = true,
                        ParentCategoryID= null,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "藝術・視覺創作",
                        IsActive = true,
                        ParentCategoryID= null,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "玩具・公仔・模型",
                        IsActive = true,
                        ParentCategoryID= null,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "香氛・保養",
                        IsActive = true,
                        ParentCategoryID= null,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "手作食品類",
                        IsActive = true,
                        ParentCategoryID= null,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "布作縫紉",//7
                        IsActive = true,
                        ParentCategoryID = 1,
                        CreatedAt = DateTime.Now 
                    },
                    new Category { CategoryName = "皮革工藝",//8
                        IsActive = true,
                        ParentCategoryID= 1,
                        CreatedAt = DateTime.Now 
                    },
                    new Category { CategoryName = "木工竹藝",//9
                        IsActive = true,
                        ParentCategoryID= 1,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "金工飾品",//10
                        IsActive = true,
                        ParentCategoryID= 1,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "陶藝黏土",//11
                        IsActive = true,
                        ParentCategoryID= 1,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "紙藝印章",//12
                        IsActive = true,
                        ParentCategoryID= 1,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "編織纖維",//13
                        IsActive = true,
                        ParentCategoryID= 1,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "乾燥・永生花",//14
                        IsActive = true,
                        ParentCategoryID= 2,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "園藝・植栽",//15
                        IsActive = true,
                        ParentCategoryID= 2,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "居家佈置小物",//16
                        IsActive = true,
                        ParentCategoryID= 2,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "環保生活手作",//17
                        IsActive = true,
                        ParentCategoryID= 2,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "插畫・畫作",//18
                        IsActive = true,
                        ParentCategoryID= 3,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "手繪商品",//19
                        IsActive = true,
                        ParentCategoryID= 3,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "版畫・拓印",//20
                        IsActive = true,
                        ParentCategoryID= 3,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "攝影藝術",//21
                        IsActive = true,
                        ParentCategoryID= 3,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "拼貼藝術",//22
                        IsActive = true,
                        ParentCategoryID= 3,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "黏土公仔",//23
                        IsActive = true,
                        ParentCategoryID= 4,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "布娃娃",//24
                        IsActive = true,
                        ParentCategoryID= 4,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "樹脂模型",//25
                        IsActive = true,
                        ParentCategoryID= 4,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "積木",//26
                        IsActive = true,
                        ParentCategoryID= 4,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "香氛",//27
                        IsActive = true,
                        ParentCategoryID= 5,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "沐浴護理",//28
                        IsActive = true,
                        ParentCategoryID= 5,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "保養護膚",//29
                        IsActive = true,
                        ParentCategoryID= 5,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "甜點與烘焙",//30
                        IsActive = true,
                        ParentCategoryID= 6,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "果醬與醬料",//31
                        IsActive = true,
                        ParentCategoryID= 6,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "飲品•茶•咖啡",//32
                        IsActive = true,
                        ParentCategoryID= 6,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "節慶禮盒",//33
                        IsActive = true,
                        ParentCategoryID= 6,
                        CreatedAt = DateTime.Now
                    },
                    new Category { CategoryName = "健康食品",//34
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
