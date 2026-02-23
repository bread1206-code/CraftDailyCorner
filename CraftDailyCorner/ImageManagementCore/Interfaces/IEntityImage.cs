namespace CraftDailyCorner.ImageManagementCore.Interfaces
{
    public interface IEntityImage
    {
        long ImageID { get;}

        // 對應 ProductID / PortfolioID
        string EntityID { get; }

        string ImageUrl { get; set; }

        byte SortOrder { get; set; }
    }
}