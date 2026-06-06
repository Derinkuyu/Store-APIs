using Microsoft.AspNetCore.Http;

namespace Store.BLL
{
    public class ImageUploadDto
    {
        /*------------------------------------------------------------------*/
        public required IFormFile File { get; set; }
    }

    public class ImageUploadResultDto
    {
        /*------------------------------------------------------------------*/
        public string ImageUrl { get; set; } = string.Empty;
    }
}
