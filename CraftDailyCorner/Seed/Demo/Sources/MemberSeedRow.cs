public class MemberSeedRow
{
    public string MemberID { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public byte Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Phone { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public byte StatusID { get; set; }
    public bool IsCreator { get; set; }
    public bool IsAdmin { get; set; }
    public string? AdminLevel { get; set; }
}