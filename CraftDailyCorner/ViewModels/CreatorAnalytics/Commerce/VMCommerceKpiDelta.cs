namespace CraftDailyCorner.ViewModels.CreatorAnalytics.Commerce
{
    public class VMCommerceKpiDelta<T>
    {
        public T Current { get; set; } = default!;
        public T Previous { get; set; } = default!;
        public decimal GrowthRate { get; set; }   // e.g. 0.12 => +12%
    }
}