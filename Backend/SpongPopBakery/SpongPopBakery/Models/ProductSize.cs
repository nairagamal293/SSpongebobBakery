namespace SpongPopBakery.Models
{
    public class ProductSize
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., "Small", "Medium", "Large"
        public string NameAr { get; set; } // Arabic name
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public bool IsDefault { get; set; } // Flag to indicate default size
    }
}
