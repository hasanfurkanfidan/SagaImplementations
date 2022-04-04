using System.Collections.Generic;

namespace Order.Api.Dtos
{
    public class OrderCreateDto
    {
        public string BuyerId { get; set; }
        public PaymentDto Payment { get; set; }
        public AdressDto Adress { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
