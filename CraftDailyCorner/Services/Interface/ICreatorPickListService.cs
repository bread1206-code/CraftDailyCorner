using CraftDailyCorner.ViewModels.CreatorPickList;

namespace CraftDailyCorner.Services.Interface
{
    public interface ICreatorPickListService
    {
        //產生撿貨單並自動將訂單改為 Processing
        Task<VMCreatorPickList?> GeneratePickListPreviewAsync(
    string creatorId,
    List<string> orderIds);

        Task<bool> ConfirmPrintAsync(
            string creatorId,
            List<string> orderIds);
    }
}
