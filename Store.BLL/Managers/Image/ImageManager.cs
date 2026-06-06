using FluentValidation;
using Store.Common;

namespace Store.BLL
{
    public class ImageManager : IImageManager
    {
        private readonly IValidator<ImageUploadDto> _validator;
        private readonly IErrorMapper _errorMapper;
        /*------------------------------------------------------------------*/
        public ImageManager(IValidator<ImageUploadDto> validator, IErrorMapper errorMapper)
        {
            _validator = validator;
            _errorMapper = errorMapper;
        }
        /*------------------------------------------------------------------*/
        public async Task<GeneralResult<ImageUploadResultDto>> UploadAsync(
            ImageUploadDto imageUploadDto,
            string basePath,
            string folderName)
        {
            var validationResult = await _validator.ValidateAsync(imageUploadDto);
            if (!validationResult.IsValid)
                return GeneralResult<ImageUploadResultDto>.FailureResult("Validation failed.", _errorMapper.MapError(validationResult));

            var uploadFolder = Path.Combine(basePath, folderName);
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var ext = Path.GetExtension(imageUploadDto.File.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await imageUploadDto.File.CopyToAsync(stream);

            var imageUrl = $"/{folderName}/{fileName}";
            return GeneralResult<ImageUploadResultDto>.SuccessResult(
                new ImageUploadResultDto { ImageUrl = imageUrl },
                "Image uploaded successfully.");
        }
    }
}
