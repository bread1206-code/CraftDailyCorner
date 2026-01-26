public enum OrderStatus : byte
{
    Pending = 0,	//未付款
    Paid = 1,	//已付款
    Shipped = 2,	//已出貨
    Completed = 3,	//已完成
    Cancelled = 4	//已取消
}