namespace SpongPopBakery.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; } // Arabic name
        public string Description { get; set; }
        public string DescriptionAr { get; set; } // Arabic description
        public string ImagePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
