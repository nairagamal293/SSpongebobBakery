namespace SpongPopBakery.DTOs
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile Image { get; set; }
        public int CategoryId { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
