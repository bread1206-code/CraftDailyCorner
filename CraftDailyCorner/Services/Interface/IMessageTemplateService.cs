using CraftDailyCorner.ViewModels.Message;

namespace CraftDailyCorner.Services.Interface
{
    public interface IMessageTemplateService
    {
        Task<VMMessageTemplateManage> GetManageVmAsync(string creatorId);

        // 只給「快速回覆模板」使用
        Task<VMMessageTemplateUpsert> GetCreateVmAsync();

        Task<VMMessageTemplateUpsert?> GetEditVmAsync(int id, string creatorId);

        // 只允許建立 QuickReply
        Task CreateAsync(VMMessageTemplateUpsert vm, string creatorId);

        // FirstMessage / QuickReply 都可編輯，但不可改 TriggerType
        Task<bool> UpdateAsync(VMMessageTemplateUpsert vm, string creatorId);

        Task<bool> EnableAsync(int id, string creatorId);

        Task<bool> DisableAsync(int id, string creatorId);
    }
}