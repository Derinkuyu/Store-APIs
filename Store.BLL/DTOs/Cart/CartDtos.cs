namespace Store.BLL
{
    public class CartItemAddDto
    {
        /*------------------------------------------------------------------*/
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartItemUpdateDto
    {
        /*------------------------------------------------------------------*/
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartItemReadDto
    {
        /*------------------------------------------------------------------*/
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string? ProductImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => ProductPrice * Quantity;
    }

    public class CartReadDto
    {
        /*------------------------------------------------------------------*/
        public IEnumerable<CartItemReadDto> Items { get; set; } = [];
        public decimal GrandTotal => Items.Sum(i => i.TotalPrice);
    }
}
