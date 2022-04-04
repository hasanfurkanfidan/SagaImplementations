namespace Order.Api.Dtos
{
    public class OrderItemDto
    {
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
