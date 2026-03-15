using System.ComponentModel.DataAnnotations;
using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.ViewModels.CreatorPortfolio
{
    public class VMCreatorPortfolioCreate : IValidatableObject
    {
        [Display(Name = "標題")]
        [Required(ErrorMessage = "請輸入標題")]
        [StringLength(50)]
        public string Title { get; set; } = null!;

        [Display(Name = "簡介")]
        [Required(ErrorMessage = "請輸入簡介")]
        public string? Description { get; set; }

        [Display(Name = "觀看權限")]
        [Required]
        public CreatorVisibility Visibility { get; set; }

        [Display(Name = "作品圖片")]
        public List<IFormFile>? Files { get; set; } = new();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Files == null || Files.Count == 0)
            {
                yield return new ValidationResult(
                    "請至少上傳 1 張圖片",
                    new[] { nameof(Files) });
                yield break;
            }

            if (Files.Count > 25)
            {
                yield return new ValidationResult(
                    "圖片最多只能上傳 25 張",
                    new[] { nameof(Files) });
            }

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg" };
            var allowedExts = new[] { ".jpg", ".jpeg", ".png" };

            foreach (var file in Files)
            {
                if (file == null || file.Length == 0)
                {
                    yield return new ValidationResult(
                        "不可上傳空白檔案",
                        new[] { nameof(Files) });
                    continue;
                }

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedTypes.Contains(file.ContentType) || !allowedExts.Contains(ext))
                {
                    yield return new ValidationResult(
                        $"檔案「{file.FileName}」不是支援的圖片格式，僅允許 JPG、JPEG、PNG",
                        new[] { nameof(Files) });
                }
            }
        }
    }
}
