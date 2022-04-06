using Shared.EventMessages;
using Shared.Orchestiration.Interfaces;
using System.Collections.Generic;

namespace Shared.Orchestiration.Events
{
    public class PaymentFailedRequestEvent : IPaymentFailedRequestEvent
    {
        public int OrderId { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
        public string Reason { get; set; }
    }
}
