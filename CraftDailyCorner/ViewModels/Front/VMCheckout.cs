using CraftDailyCorner.ViewModels.Front;
using System.Collections.Generic;

namespace CraftDailyCorner.ViewModels.Front
{
    public class VMCheckout
    {

        public List<VMCartItem> Items { get; set; } = new();

        public int TotalAmount { get; set; }
    }
}
