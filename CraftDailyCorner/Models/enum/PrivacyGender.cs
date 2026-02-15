using System.ComponentModel.DataAnnotations;

public enum PrivacyGender: byte
{
    [Display(Name = "男生")]
    Male=0,     //男生
    [Display(Name = "女生")]
    Female =1,   //女生
    [Display(Name = "其他")]
    Other =2     //其他
}

