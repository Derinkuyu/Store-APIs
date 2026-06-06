using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BLL;
using Store.Common;

namespace Store.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        /*------------------------------------------------------------------*/
        private readonly IProductManager _productManager;
        /*------------------------------------------------------------------*/
        public ProductsController(IProductManager productManager)
        {
            _productManager = productManager;
        }
        /*------------------------------------------------------------------*/
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductFilterParameters filters)
        {
            var result = await _productManager.GetProductsAsync(filters);
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productManager.GetProductByIdAsync(id);
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            var result = await _productManager.CreateProductAsync(dto);
            if (!result.Success)
                return BadRequest(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }
        /*------------------------------------------------------------------*/
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductEditDto dto)
        {
            var result = await _productManager.UpdateProductAsync(id, dto);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productManager.DeleteProductAsync(id);
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
        [HttpPost("{id}/image")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UploadProductImage(int id, [FromServices] IImageManager imageManager, [FromForm] ImageUploadDto dto)
        {
            var product = await _productManager.GetProductByIdAsync(id);
            if (!product.Success)
                return NotFound(product);

            var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadResult = await imageManager.UploadAsync(dto, webRootPath, "images/products");
            if (!uploadResult.Success)
                return BadRequest(uploadResult);

            var updateDto = new ProductEditDto
            {
                Name = product.Data!.Name,
                Description = product.Data.Description,
                Price = product.Data.Price,
                Stock = product.Data.Stock,
                CategoryId = product.Data.CategoryId
            };
            // Update product image URL directly
            var updated = await _productManager.UpdateProductAsync(id, updateDto);
            return Ok(uploadResult);
        }
    }
}
