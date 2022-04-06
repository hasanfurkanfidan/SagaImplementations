using MassTransit;
using Shared.EventMessages;
using System;
using System.Collections.Generic;

namespace Shared.Orchestiration.Interfaces
{
    public interface IPaymentFailedEvent : CorrelatedBy<Guid>
    {
        public List<OrderItemMessage> OrderItems { get; set; }
        public string Reason { get; set; }
    }
}
