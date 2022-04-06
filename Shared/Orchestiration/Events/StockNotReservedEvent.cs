using Shared.Orchestiration.Interfaces;
using System;

namespace Shared.Orchestiration.Events
{
    public class StockNotReservedEvent : IStockNotReservedEvent
    {
        public StockNotReservedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public string Reason { get; set; }

        public Guid CorrelationId { get; }
    }
}
