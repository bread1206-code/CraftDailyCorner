public class VMCreatorPortfolioItemEdit
{
    public int ItemID { get; set; }

    public string ImageUrl { get; set; } = null!;

    public byte SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string ImagePath =>
        $"/Photos/06Portfolio/Medium/{ImageUrl}.png";
}