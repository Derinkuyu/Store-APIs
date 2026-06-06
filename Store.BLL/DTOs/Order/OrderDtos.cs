using Store.DAL;

namespace Store.BLL
{
    public class OrderReadDto
    {
        /*------------------------------------------------------------------*/
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<OrderItemReadDto> OrderItems { get; set; } = [];
    }

    public class OrderItemReadDto
    {
        /*------------------------------------------------------------------*/
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal => UnitPrice * Quantity;
    }
}
