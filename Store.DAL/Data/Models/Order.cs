namespace Store.DAL
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    public class Order : IAuditableEntity
    {
        /*------------------------------------------------------------------*/
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        /*------------------------------------------------------------------*/
        // Navigation
        public AppUser? User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = [];
    }
}
