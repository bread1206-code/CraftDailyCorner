public class VMCreatorPortfolioItemEdit
{
    public long ItemID { get; set; }

    public string CreatorID { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public byte SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string ImagePath =>
        $"/Photos/06Portfolio/{CreatorID}/Large/{ImageUrl}.webp";
}