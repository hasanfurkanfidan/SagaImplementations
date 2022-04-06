using Shared.Orchestiration.Interfaces;
using System;

namespace Shared.Orchestiration.Events
{
    public class PaymentCompletedEvent : IPaymentCompletedEvent
    {
        public PaymentCompletedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; set; }
    }
}
