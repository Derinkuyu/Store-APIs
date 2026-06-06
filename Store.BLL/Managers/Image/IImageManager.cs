using Store.Common;

namespace Store.BLL
{
    public interface IImageManager
    {
        /*------------------------------------------------------------------*/
        Task<GeneralResult<ImageUploadResultDto>> UploadAsync(ImageUploadDto imageUploadDto, string basePath, string folderName);
    }
}
