using System;

namespace SmartInventorySystem.Domain.Entities
{
    public class Sale
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime Date { get; set; }
    }
}