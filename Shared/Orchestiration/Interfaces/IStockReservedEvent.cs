using MassTransit;
using Shared.EventMessages;
using System;
using System.Collections.Generic;

namespace Shared.Orchestiration.Interfaces
{
    public interface IStockReservedEvent : CorrelatedBy<Guid>
    {
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
