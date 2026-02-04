namespace CraftDailyCorner.ViewModels.Front
{
    //下單完成後回傳給前端 / 導頁用
    public class VMCreateOrderResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public string? OrderID { get; set; }
    }
}
