public enum InventoryAlertStatus : byte
{
    Pending = 0,	//尚未觸發
    Triggered = 1,	//已提醒
    Resolved = 2	//已處理
}