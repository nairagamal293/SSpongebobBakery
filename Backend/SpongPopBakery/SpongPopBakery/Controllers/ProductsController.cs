using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpongPopBakery.DTOs;
using SpongPopBakery.Models;
using SpongPopBakery.Services;

namespace SpongPopBakery.Controllers
{
    // Controllers/ProductsController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IImageService _imageService;
        private readonly ICategoryService _categoryService;

        public ProductsController(
            IProductService productService,
            IImageService imageService,
            ICategoryService categoryService)
        {
            _productService = productService;
            _imageService = imageService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            var productsDto = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                NameAr = p.NameAr,
                Description = p.Description,
                DescriptionAr = p.DescriptionAr,
                Price = p.Price,
                ImageUrl = _imageService.GetImageUrl(p.ImagePath),
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                IsAvailable = p.IsAvailable
            });
            return Ok(productsDto);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategory(categoryId);
            var productsDto = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                NameAr = p.NameAr,
                Description = p.Description,
                DescriptionAr = p.DescriptionAr,
                Price = p.Price,
                ImageUrl = _imageService.GetImageUrl(p.ImagePath),
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                IsAvailable = p.IsAvailable
            });
            return Ok(productsDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto productCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _categoryService.GetCategoryById(productCreateDto.CategoryId);
            if (category == null)
                return BadRequest("Invalid category ID");

            var imagePath = await _imageService.SaveImage(productCreateDto.Image, "products");

            var product = new Product
            {
                Name = productCreateDto.Name,
                NameAr = productCreateDto.NameAr,
                Description = productCreateDto.Description,
                DescriptionAr = productCreateDto.DescriptionAr,
                Price = productCreateDto.Price,
                ImagePath = imagePath,
                CategoryId = productCreateDto.CategoryId,
                IsAvailable = productCreateDto.IsAvailable
            };

            var createdProduct = await _productService.CreateProduct(product);

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, new ProductDto
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                NameAr = createdProduct.NameAr,
                Description = createdProduct.Description,
                DescriptionAr = createdProduct.DescriptionAr,
                Price = createdProduct.Price,
                ImageUrl = _imageService.GetImageUrl(createdProduct.ImagePath),
                CategoryId = createdProduct.CategoryId,
                CategoryName = category.Name,
                IsAvailable = createdProduct.IsAvailable
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
                return NotFound();

            return Ok(new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                NameAr = product.NameAr,
                Description = product.Description,
                DescriptionAr = product.DescriptionAr,
                Price = product.Price,
                ImageUrl = _imageService.GetImageUrl(product.ImagePath),
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                IsAvailable = product.IsAvailable
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductCreateDto productUpdateDto)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
                return NotFound();

            // Only update image if a new one was provided
            if (productUpdateDto.Image != null)
            {
                _imageService.DeleteImage(product.ImagePath);
                product.ImagePath = await _imageService.SaveImage(productUpdateDto.Image, "products");
            }

            product.Name = productUpdateDto.Name;
            product.NameAr = productUpdateDto.NameAr;
            product.Description = productUpdateDto.Description;
            product.DescriptionAr = productUpdateDto.DescriptionAr;
            product.Price = productUpdateDto.Price;
            product.CategoryId = productUpdateDto.CategoryId;
            product.IsAvailable = productUpdateDto.IsAvailable;
            product.UpdatedAt = DateTime.UtcNow;

            await _productService.UpdateProduct(product);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
                return NotFound();

            _imageService.DeleteImage(product.ImagePath);
            await _productService.DeleteProduct(id);

            return NoContent();
        }
    }
}
