using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels
{
    public class VMLogin
    {
        [Key]
        [Display(Name = "帳號")]
        [Required(ErrorMessage = "必填")]
        [MaxLength(40)]
        public string Account { get; set; } = null!;

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "必填")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "密碼為3-20碼")]
        [DataType(DataType.Password)]
        [MaxLength(20)]
        public string Password { get; set; } = null!;

        public bool RememberAccount { get; set; }
    }
}
