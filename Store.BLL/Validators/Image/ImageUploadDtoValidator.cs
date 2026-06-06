using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Store.BLL
{
    public class ImageUploadDtoValidator : AbstractValidator<ImageUploadDto>
    {
        private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public ImageUploadDtoValidator()
        {
            RuleFor(i => i.File)
                .NotNull().WithMessage("File is required.").WithErrorCode("ERR-01")
                .Must(f => f != null && f.Length > 0).WithMessage("File cannot be empty.").WithErrorCode("ERR-02")
                .Must(f => f == null || f.Length <= MaxFileSize).WithMessage("File size must not exceed 5MB.").WithErrorCode("ERR-03")
                .Must(f =>
                {
                    if (f == null) return true;
                    var ext = Path.GetExtension(f.FileName).ToLowerInvariant();
                    return _allowedExtensions.Contains(ext);
                })
                .WithMessage("Only image files (.jpg, .jpeg, .png, .gif, .webp) are allowed.").WithErrorCode("ERR-04");
        }
    }
}
