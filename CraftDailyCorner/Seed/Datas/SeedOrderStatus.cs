using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedOrderStatus
    {
        private readonly CraftDailyCornerContext _context;

        public SeedOrderStatus(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.OrderStatuses.Any()) return;

            _context.OrderStatuses.AddRange(
                new OrderStatus
                {
                    StatusID = 1,
                    StatusCode = "Pending",
                    StatusName = "待付款",
                    Description = "訂單尚未付款",
                    IsActive = true
                },
                new OrderStatus
                {
                    StatusID = 2,
                    StatusCode = "Paid",
                    StatusName = "已付款",
                    Description = "訂單已付款",
                    IsActive = true
                },
                new OrderStatus
                {
                    StatusID = 3,
                    StatusCode = "Processing",
                    StatusName = "準備中",
                    Description = "商品準備中",
                    IsActive = true
                },
                new OrderStatus
                {
                    StatusID = 4,
                    StatusCode = "Shipped",
                    StatusName = "已出貨",
                    Description = "商品已出貨",
                    IsActive = true
                },
                new OrderStatus
                {
                    StatusID = 5,
                    StatusCode = "Completed",
                    StatusName = "完成",
                    Description = "訂單完成",
                    IsActive = false
                },
                new OrderStatus
                {
                    StatusID = 6,
                    StatusCode = "Cancelled",
                    StatusName = "取消",
                    Description = "訂單取消",
                    IsActive = false
                }
            );

            _context.SaveChanges();
        }
    }

}
