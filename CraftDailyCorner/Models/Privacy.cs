using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.Models
{
    public class Privacy
    {
        [Key]
        [Display(Name = "會員編號")]
        public string MemberID { get; set; } = null!;
        
        [Required(ErrorMessage = "必填")]
        [StringLength(200)]
        [EmailAddress(ErrorMessage ="電子郵件格式錯誤")]
        [Display(Name = "電子郵件")]
        public string Email { get; set; } = null!;

        [StringLength(200)]
        [Display(Name = "密碼雜湊值")]
        [HiddenInput]
        public string PasswordHash { get; set; } = null!;

        [Column(TypeName = "nchar(10)")]
        [RegularExpression("[0][9][0-9]{8}", ErrorMessage = "手機格式錯誤")]
        [Display(Name = "手機號碼")]
        public string? Phone { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        [Display(Name = "生日")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "性別")]
        public PrivacyGender Gender { get; set; } 

        public Member Member { get; set; } = null!;
    }
}
