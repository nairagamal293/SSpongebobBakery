using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpongPopBakery.DTOs;
using SpongPopBakery.Models;
using SpongPopBakery.Services;
using System.Data;

namespace SpongPopBakery.Controllers
{
    // Controllers/CategoriesController.cs
   
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imageService;

        public CategoriesController(ICategoryService categoryService, IImageService imageService)
        {
            _categoryService = categoryService;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            var categoriesDto = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = _imageService.GetImageUrl(c.ImagePath)
            });
            return Ok(categoriesDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateDto categoryCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imagePath = await _imageService.SaveImage(categoryCreateDto.Image, "categories");

            var category = new Category
            {
                Name = categoryCreateDto.Name,
                Description = categoryCreateDto.Description,
                ImagePath = imagePath
            };

            var createdCategory = await _categoryService.CreateCategory(category);

            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, new CategoryDto
            {
                Id = createdCategory.Id,
                Name = createdCategory.Name,
                Description = createdCategory.Description,
                ImageUrl = _imageService.GetImageUrl(createdCategory.ImagePath)
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound();

            return Ok(new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = _imageService.GetImageUrl(category.ImagePath)
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryCreateDto categoryUpdateDto)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound();

            if (categoryUpdateDto.Image != null)
            {
                _imageService.DeleteImage(category.ImagePath);
                category.ImagePath = await _imageService.SaveImage(categoryUpdateDto.Image, "categories");
            }

            category.Name = categoryUpdateDto.Name;
            category.Description = categoryUpdateDto.Description;
            category.UpdatedAt = DateTime.UtcNow;

            await _categoryService.UpdateCategory(category);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound();

            _imageService.DeleteImage(category.ImagePath);
            await _categoryService.DeleteCategory(id);

            return NoContent();
        }
    }
}
