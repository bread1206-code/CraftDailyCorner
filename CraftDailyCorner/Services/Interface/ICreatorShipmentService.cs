namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorShipmentService
    {
        //產生建議物流編號（尚未寫入資料庫）
        Task<string> GenerateTrackingNoAsync();
    }
}
