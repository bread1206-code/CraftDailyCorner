public enum PaymentStatus : byte
{
    Pending = 0,	//尚未付款
    Success = 1,	//付款成功
    Failed = 2,	//付款失敗
    Cancelled = 3	//已取消
}