using Shared.EventMessages;
using Shared.Orchestiration.Interfaces;
using System;
using System.Collections.Generic;

namespace Shared.Orchestiration.Events
{
    public class PaymentFailedEvent : IPaymentFailedEvent
    {
        public PaymentFailedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public List<OrderItemMessage> OrderItems { get; set; }
        public string Reason { get; set; }

        public Guid CorrelationId { get; }
    }
}
