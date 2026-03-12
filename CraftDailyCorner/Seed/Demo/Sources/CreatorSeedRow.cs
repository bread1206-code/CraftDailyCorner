public class CreatorSeedRow
{
    public string MemberID { get; set; } = null!;
    public string CreatorID { get; set; } = null!;
    public string BrandName { get; set; } = null!;
    public string BrandIntro { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public int ApplicationOffsetDays { get; set; }
    public int ReviewOffsetDays { get; set; }
    public int ConfirmOffsetDays { get; set; }
    public string? BankCode { get; set; }
    public string? BankAccount { get; set; }
}