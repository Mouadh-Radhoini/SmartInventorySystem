using System;

namespace SmartInventorySystem.Domain.Entities
{
    public class StockMovement
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }

        public string Type { get; set; } = string.Empty; // "IN" / "OUT"

        public DateTime Date { get; set; }
    }
}