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
                ImageUrl = _imageService.GetImageUrl(p.ImagePath),
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                IsAvailable = p.IsAvailable,
                Sizes = p.Sizes.Select(s => new ProductSizeDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    NameAr = s.NameAr,
                    Price = s.Price,
                    IsDefault = s.IsDefault
                }).ToList()
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
                ImageUrl = _imageService.GetImageUrl(p.ImagePath),
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                IsAvailable = p.IsAvailable,
                Sizes = p.Sizes.Select(s => new ProductSizeDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    NameAr = s.NameAr,
                    Price = s.Price,
                    IsDefault = s.IsDefault
                }).ToList()
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
                ImagePath = imagePath,
                CategoryId = productCreateDto.CategoryId,
                IsAvailable = productCreateDto.IsAvailable,
                Sizes = productCreateDto.Sizes.Select(s => new ProductSize
                {
                    Name = s.Name,
                    NameAr = s.NameAr,
                    Price = s.Price,
                    IsDefault = s.IsDefault
                }).ToList()
            };

            // If no sizes provided, then create a default one
            if (!product.Sizes.Any())
            {
                product.Sizes.Add(new ProductSize
                {
                    Name = "Standard",
                    NameAr = "قياسي",
                    Price = 0,
                    IsDefault = true
                });
            }
            // If sizes are provided but none are marked as default, mark the first one as default
            else if (!product.Sizes.Any(s => s.IsDefault))
            {
                product.Sizes.First().IsDefault = true;
            }

            var createdProduct = await _productService.CreateProduct(product);

            var productDto = new ProductDto
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                NameAr = createdProduct.NameAr,
                Description = createdProduct.Description,
                DescriptionAr = createdProduct.DescriptionAr,
                ImageUrl = _imageService.GetImageUrl(createdProduct.ImagePath),
                CategoryId = createdProduct.CategoryId,
                CategoryName = category?.Name,
                CategoryNameAr = category?.NameAr,
                IsAvailable = createdProduct.IsAvailable,
                Sizes = createdProduct.Sizes.Select(s => new ProductSizeDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    NameAr = s.NameAr,
                    Price = s.Price,
                    IsDefault = s.IsDefault
                }).ToList()
            };

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, productDto);
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
                ImageUrl = _imageService.GetImageUrl(product.ImagePath),
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                IsAvailable = product.IsAvailable,
                Sizes = product.Sizes.Select(s => new ProductSizeDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    NameAr = s.NameAr,
                    Price = s.Price,
                    IsDefault = s.IsDefault
                }).ToList()
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateDto productUpdateDto)
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

            // Update other fields
            product.Name = productUpdateDto.Name;
            product.NameAr = productUpdateDto.NameAr;
            product.Description = productUpdateDto.Description;
            product.DescriptionAr = productUpdateDto.DescriptionAr;
            product.CategoryId = productUpdateDto.CategoryId;
            product.IsAvailable = productUpdateDto.IsAvailable;
            product.UpdatedAt = DateTime.UtcNow;

            // Clear existing sizes and add new ones
            product.Sizes.Clear();
            foreach (var sizeDto in productUpdateDto.Sizes)
            {
                product.Sizes.Add(new ProductSize
                {
                    Name = sizeDto.Name,
                    NameAr = sizeDto.NameAr,
                    Price = sizeDto.Price,
                    IsDefault = sizeDto.IsDefault
                });
            }

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
