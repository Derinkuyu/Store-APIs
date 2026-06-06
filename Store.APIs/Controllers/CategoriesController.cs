using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.BLL;

namespace Store.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        /*------------------------------------------------------------------*/
        private readonly ICategoryManager _categoryManager;
        /*------------------------------------------------------------------*/
        public CategoriesController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }
        /*------------------------------------------------------------------*/
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryManager.GetAllCategoriesAsync();
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _categoryManager.GetCategoryByIdAsync(id);
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            var result = await _categoryManager.CreateCategoryAsync(dto);
            if (!result.Success)
                return BadRequest(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }
        /*------------------------------------------------------------------*/
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryEditDto dto)
        {
            var result = await _categoryManager.UpdateCategoryAsync(id, dto);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
        /*------------------------------------------------------------------*/
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryManager.DeleteCategoryAsync(id);
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }
    }
}
