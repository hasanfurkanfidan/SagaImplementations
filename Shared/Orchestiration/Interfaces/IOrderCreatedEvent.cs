using MassTransit;
using Shared.EventMessages;
using System;
using System.Collections.Generic;

namespace Shared.Orchestiration.Interfaces
{
    public interface IOrderCreatedEvent : CorrelatedBy<Guid>
    {
        List<OrderItemMessage> OrderItems { get; set; }
    }
}
