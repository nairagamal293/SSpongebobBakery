﻿namespace SpongPopBakery.DTOs
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public IFormFile Image { get; set; }
        public int CategoryId { get; set; }
        public bool IsAvailable { get; set; } = true;
        public List<ProductSizeCreateDto> Sizes { get; set; } = new List<ProductSizeCreateDto>();
    }
}
