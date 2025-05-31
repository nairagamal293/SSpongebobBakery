namespace SpongPopBakery.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; } // Arabic name
        public string Description { get; set; }
        public string DescriptionAr { get; set; } // Arabic description
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
