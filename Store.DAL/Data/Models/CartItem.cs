namespace Store.DAL
{
    public class CartItem
    {
        /*------------------------------------------------------------------*/
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        /*------------------------------------------------------------------*/
        // Navigation
        public AppUser? User { get; set; }
        public Product? Product { get; set; }
    }
}
