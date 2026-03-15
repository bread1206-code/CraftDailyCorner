using CraftDailyCorner.Seed.Demo.Sources;
using System.Globalization;
using System.Text;

namespace CraftDailyCorner.Seed.Demo.Loaders
{
    public class OrderSeedLoader
    {
        public List<OrderSeedRow> LoadOrders(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
                throw new ArgumentException("CSV 路徑不可為空");

            if (!File.Exists(csvPath))
                throw new FileNotFoundException("找不到 Orders.csv", csvPath);

            var lines = File.ReadAllLines(csvPath, Encoding.UTF8);

            if (lines.Length <= 1)
                return new List<OrderSeedRow>();

            var rows = new List<OrderSeedRow>();

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');

                if (parts.Length < 9)
                    throw new Exception($"Orders.csv 欄位數不足：{line}");

                rows.Add(new OrderSeedRow
                {
                    OrderID = parts[0].Trim(),
                    ReceiverName = parts[1].Trim(),
                    ReceiverPhone = parts[2].Trim(),
                    ShippingAddress = parts[3].Trim(),
                    CreatedAt = DateTime.Parse(parts[4].Trim(), CultureInfo.InvariantCulture),
                    UpdatedAt = DateTime.Parse(parts[5].Trim(), CultureInfo.InvariantCulture),
                    StatusID = byte.Parse(parts[6].Trim()),
                    TotalAmount = decimal.Parse(parts[7].Trim(), CultureInfo.InvariantCulture),
                    MemberID = parts[8].Trim()
                });
            }

            return rows;
        }

        public List<OrderDetailSeedRow> LoadOrderDetails(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
                throw new ArgumentException("CSV 路徑不可為空");

            if (!File.Exists(csvPath))
                throw new FileNotFoundException("找不到 OrderDetails.csv", csvPath);

            var lines = File.ReadAllLines(csvPath, Encoding.UTF8);

            if (lines.Length <= 1)
                return new List<OrderDetailSeedRow>();

            var rows = new List<OrderDetailSeedRow>();

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');

                if (parts.Length < 6)
                    throw new Exception($"OrderDetails.csv 欄位數不足：{line}");

                rows.Add(new OrderDetailSeedRow
                {
                    OrderID = parts[0].Trim(),
                    ProductID = parts[1].Trim(),
                    ProductNameSnapshot = parts[2].Trim(),
                    PriceSnapshot = decimal.Parse(parts[3].Trim(), CultureInfo.InvariantCulture),
                    CostSnapshot = decimal.Parse(parts[4].Trim(), CultureInfo.InvariantCulture),
                    Quantity = int.Parse(parts[5].Trim())
                });
            }

            return rows;
        }

        public List<PaymentSeedRow> LoadPayments(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
                throw new ArgumentException("CSV 路徑不可為空");

            if (!File.Exists(csvPath))
                throw new FileNotFoundException("找不到 Payments.csv", csvPath);

            var lines = File.ReadAllLines(csvPath, Encoding.UTF8);

            if (lines.Length <= 1)
                return new List<PaymentSeedRow>();

            var rows = new List<PaymentSeedRow>();

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');

                if (parts.Length < 9)
                    throw new Exception($"Payments.csv 欄位數不足：{line}");

                rows.Add(new PaymentSeedRow
                {
                    PaymentID = int.Parse(parts[0].Trim()),
                    MethodID = byte.Parse(parts[1].Trim()),
                    Amount = decimal.Parse(parts[2].Trim(), CultureInfo.InvariantCulture),
                    StatusID = byte.Parse(parts[3].Trim()),
                    GatewayTradeNo = parts[4].Trim(),
                    AttemptNo = byte.Parse(parts[5].Trim()),
                    PaidAt = ParseNullableDateTime(parts[6]),
                    CreatedAt = DateTime.Parse(parts[7].Trim(), CultureInfo.InvariantCulture),
                    OrderID = parts[8].Trim()
                });
            }

            return rows;
        }

        public List<ShipmentSeedRow> LoadShipments(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
                throw new ArgumentException("CSV 路徑不可為空");

            if (!File.Exists(csvPath))
                throw new FileNotFoundException("找不到 Shipments.csv", csvPath);

            var lines = File.ReadAllLines(csvPath, Encoding.UTF8);

            if (lines.Length <= 1)
                return new List<ShipmentSeedRow>();

            var rows = new List<ShipmentSeedRow>();

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');

                if (parts.Length < 6)
                    throw new Exception($"Shipments.csv 欄位數不足：{line}");

                rows.Add(new ShipmentSeedRow
                {
                    ShipmentID = int.Parse(parts[0].Trim()),
                    TrackingNo = parts[1].Trim(),
                    StatusID = byte.Parse(parts[2].Trim()),
                    ShippedAt = ParseNullableDateTime(parts[3]),
                    DeliveredAt = ParseNullableDateTime(parts[4]),
                    OrderID = parts[5].Trim()
                });
            }

            return rows;
        }

        private static DateTime? ParseNullableDateTime(string value)
        {
            var text = value?.Trim();

            if (string.IsNullOrWhiteSpace(text))
                return null;

            return DateTime.Parse(text, CultureInfo.InvariantCulture);
        }
    }
}