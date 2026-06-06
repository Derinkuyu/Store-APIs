using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BLL;

namespace Store.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        /*------------------------------------------------------------------*/
        private readonly IImageManager _imageManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        /*------------------------------------------------------------------*/
        public ImagesController(IImageManager imageManager, IWebHostEnvironment webHostEnvironment)
        {
            _imageManager = imageManager;
            _webHostEnvironment = webHostEnvironment;
        }
        /*------------------------------------------------------------------*/
        [HttpPost("upload")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadDto dto)
        {
            var basePath = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var result = await _imageManager.UploadAsync(dto, basePath, "images");
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
        [HttpPost("products/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UploadProductImage(int id, [FromForm] ImageUploadDto dto, [FromServices] IProductManager productManager)
        {
            var product = await productManager.GetProductByIdAsync(id);
            if (!product.Success)
                return NotFound(product);

            var basePath = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var result = await _imageManager.UploadAsync(dto, basePath, "images/products");
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
        [HttpPost("categories/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UploadCategoryImage(int id, [FromForm] ImageUploadDto dto, [FromServices] ICategoryManager categoryManager)
        {
            var category = await categoryManager.GetCategoryByIdAsync(id);
            if (!category.Success)
                return NotFound(category);

            var basePath = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var result = await _imageManager.UploadAsync(dto, basePath, "images/categories");
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
