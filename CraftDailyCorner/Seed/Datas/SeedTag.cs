using CraftDailyCorner.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
                // 1. 將所有標籤集中在字串清單中，方便未來直接增刪文字即可
                var tagNames = new List<string>
                {
                    
                    "手工", "限量", "原創", "客製化", "手作課程體驗", "親子活動套組", "原木",
                    
                    // --- 基礎屬性 ---
                    "純銀", "真皮", "實木", "亞麻", "天然石", "天然皮革", "純銅鍛造", "天然植物染", "手抄紙",
                    "手縫", "手繪", "陶藝", "編織", "雷雕", "翻模", "木工", "手工刺繡", "木雕工藝", "金工創作", "羊毛氈",
                    "飾品", "包包提袋", "文具小物", "寵物用品", "乾燥花藝", "微型模型",

                    // --- 風格與美學 ---
                    "極簡風", "極簡主義", "復古懷舊", "工業風", "森林系", "波西米亞", "日系和風", "北歐簡約", "現代摩登", "禪意美學",
                    "侘寂風", "街頭潮流", "療癒系", "文藝清新", "文青必備", "童趣可愛", "輕奢華", "暗黑美學", "大膽撞色", "優雅古典",
                    "透明感", "質感生活", "大地色系", "馬卡龍色", "莫蘭迪色",

                    // --- 場景與功能 ---
                    "居家裝飾", "辦公室好物", "辦公療癒", "隨身配件", "餐桌美學", "戶外生活", "野餐必備", "旅行夥伴", "牆面藝術", "手帳必備", "節慶裝飾",
                    "收納好物", "個人穿搭", "防水", "輕量化", "多功能", "防抗敏", "香氛舒壓",

                    // --- 送禮與客製化 ---
                    "客製化禮物", "刻字服務", "手工訂製", "獨一無二", "限量供應", "精緻包裝", "手工卡片",
                    "情人節禮物", "生日送禮", "交換禮物", "母親節推薦", "伴手禮", "畢業禮物", "彌月禮", "入厝禮", "新婚賀禮", "婚禮小物", "企業送禮",
                    "送女友", "送男生", "新手爸媽", "閨蜜禮物",

                    // --- 品牌理念與情感共鳴 ---
                    "職人精神", "匠心獨具", "在地生產", "慢生活", "手作的溫度", "時間的痕跡", "暖心設計", "具儀式感",
                    "友善環境", "永續設計", "無塑料", "純植物性", "純素食", "環保永續", "手感溫度", "天然香氣", "親膚材質"
                };

                // 2. 透過 LINQ 批次轉換為 Tag 物件，並使用 Distinct() 再次確保資料庫不會寫入重複名稱
                var tags = tagNames.Distinct().Select(name => new Tag
                {
                    TagName = name,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                }).ToList();

                _context.Tags.AddRange(tags);
                _context.SaveChanges();
            }
        }
    }
}