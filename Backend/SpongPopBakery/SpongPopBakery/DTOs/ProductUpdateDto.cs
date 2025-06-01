namespace SpongPopBakery.DTOs
{
    public class ProductUpdateDto
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public IFormFile? Image { get; set; } // Nullable for optional updates
        public int CategoryId { get; set; }
        public bool IsAvailable { get; set; }
        public List<ProductSizeCreateDto> Sizes { get; set; } = new List<ProductSizeCreateDto>();
    }
}
