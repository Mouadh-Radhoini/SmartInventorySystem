namespace SmartInventorySystem.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        // Basic fields
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // Foreign key
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Low-stock threshold
        public int MinStock { get; set; } = 5;

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}