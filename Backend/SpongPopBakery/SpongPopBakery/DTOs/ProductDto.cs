namespace SpongPopBakery.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryNameAr { get; set; }
        public bool IsAvailable { get; set; }
        public List<ProductSizeDto> Sizes { get; set; } = new List<ProductSizeDto>();

        // Helper property for backward compatibility
        public decimal? Price => Sizes.FirstOrDefault(s => s.IsDefault)?.Price;
    }

}
