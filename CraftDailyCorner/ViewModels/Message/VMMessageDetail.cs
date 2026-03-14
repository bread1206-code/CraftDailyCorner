using System.Collections.Generic;

namespace CraftDailyCorner.ViewModels.Message
{
    public class VMMessageDetail
    {
        public int ThreadID { get; set; }

        // 對話對象
        public string DisplayName { get; set; } = null!;
        public string? SubTitle { get; set; }

        // 商品上下文卡片
        public string? ProductID { get; set; }
        public string? ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public string? ProductImageUrl { get; set; }

        public string? ProductCreatorID { get; set; }

        public string ProductImagePath =>
            string.IsNullOrWhiteSpace(ProductImageUrl) || string.IsNullOrWhiteSpace(ProductCreatorID)
                ? "/images/no-image.png"
                : $"/Photos/04ProductImage/{ProductCreatorID}/Medium/{ProductImageUrl}.png";

        // 訊息清單
        public List<VMMessageItem> Messages { get; set; } = new();

        // 快速回覆模板
        public List<VMQuickReplyTemplateItem> QuickReplyTemplates { get; set; } = new();

        // 輸入框預設內容
        public string? DraftContent { get; set; }

        public bool IsCreatorThread { get; set; }
    }
}