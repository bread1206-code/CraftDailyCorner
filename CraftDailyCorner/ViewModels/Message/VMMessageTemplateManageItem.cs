namespace CraftDailyCorner.ViewModels.Message
{
    public class VMMessageTemplateManageItem
    {
        public int TemplateID { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public byte TriggerType { get; set; }

        public string TriggerTypeName { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}