namespace Store.DAL
{
    public class Product : IAuditableEntity
    {
        /*------------------------------------------------------------------*/
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        /*------------------------------------------------------------------*/
        // FK
        public int CategoryId { get; set; }
        // Navigation
        public Category? Category { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = [];
        public ICollection<OrderItem> OrderItems { get; set; } = [];
    }
}
