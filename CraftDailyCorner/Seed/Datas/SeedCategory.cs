using CraftDailyCorner.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
                var now = DateTime.Now;

                // 1. 用 Dictionary 建立清晰的「主分類 -> 子分類」對應表
                // Key = 主分類名稱, Value = 該主分類下的子分類名稱清單
                var categoryHierarchy = new Dictionary<string, List<string>>
                {
                    { "手作工藝類", new List<string> { "布作縫紉", "皮革工藝", "木工竹藝", "金工飾品", "陶藝黏土", "紙藝印章", "編織纖維" } },
                    { "園藝・生活空間", new List<string> { "乾燥・永生花", "園藝・植栽", "居家佈置小物", "環保生活手作" } },
                    { "藝術・視覺創作", new List<string> { "插畫・畫作", "手繪商品", "版畫・拓印", "攝影藝術", "拼貼藝術" } },
                    { "玩具・公仔・模型", new List<string> { "黏土公仔", "布娃娃", "樹脂模型", "積木" } },
                    { "香氛・保養", new List<string> { "香氛", "沐浴護理", "保養護膚" } },
                    { "手作食品類", new List<string> { "甜點與烘焙", "果醬與醬料", "飲品•茶•咖啡", "節慶禮盒", "健康食品" } }
                };

                // 2. 迴圈處理階層寫入
                foreach (var item in categoryHierarchy)
                {
                    // 先建立主分類
                    var parentCategory = new Category
                    {
                        CategoryName = item.Key,
                        IsActive = true,
                        ParentCategoryID = null,
                        CreatedAt = now
                    };

                    _context.Categories.Add(parentCategory);

                    // 必須先 SaveChanges()，資料庫才會配發 ID 給這筆主分類
                    _context.SaveChanges();

                    // 建立該主分類底下的所有子分類，並動態綁定剛剛生成的 ID
                    var childCategories = item.Value.Select(childName => new Category
                    {
                        CategoryName = childName,
                        IsActive = true,
                        ParentCategoryID = parentCategory.CategoryID,
                        CreatedAt = now
                    }).ToList();

                    _context.Categories.AddRange(childCategories);
                }

                // 儲存所有子分類
                _context.SaveChanges();
            }
        }
    }
}