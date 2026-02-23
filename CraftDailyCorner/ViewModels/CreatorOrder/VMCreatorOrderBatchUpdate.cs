namespace CraftDailyCorner.ViewModels.CreatorOrder
{
    public class VMCreatorOrderBatchUpdate
    {
        public List<string> SelectedOrderIDs { get; set; } = new();

        public byte TargetStatusID { get; set; }
    }
}
