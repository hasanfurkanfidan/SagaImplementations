using Shared.EventMessages;
using System.Collections.Generic;

namespace Shared.Event
{
    public class PaymentFailedEvent
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
        public string FailMessage { get; set; }
        public List<OrderItemMessage> OrderItemMessages { get; set; }
    }
}
