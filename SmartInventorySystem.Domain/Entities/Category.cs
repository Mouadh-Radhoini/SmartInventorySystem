namespace SmartInventorySystem.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }              // Unique ID
        public string Name { get; set; }         // Category name
        public List<Product> Products { get; set; } = new(); 
    }
}