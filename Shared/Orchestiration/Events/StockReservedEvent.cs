using Shared.EventMessages;
using Shared.Orchestiration.Interfaces;
using System;
using System.Collections.Generic;

namespace Shared.Orchestiration.Event
{
    public class StockReservedEvent : IStockReservedEvent
    {
        public StockReservedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public List<OrderItemMessage> OrderItems { get; set; }
        public Guid CorrelationId { get; }
    }
}
