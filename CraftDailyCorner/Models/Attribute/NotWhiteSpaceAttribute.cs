using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Models.Attribute
{
    public class NotWhiteSpaceAttribute : ValidationAttribute
    {
        //自訂驗證屬性
        //檢查字串是否為空白字元
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string str)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return new ValidationResult(ErrorMessage ?? "不可輸入空白字元");
                }
            }
            return ValidationResult.Success;
        }
    }
}
