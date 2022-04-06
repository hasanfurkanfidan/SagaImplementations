using Shared.Orchestiration.Interfaces;

namespace Shared.Orchestiration.Events
{
    public class StockNotReservedRequestEvent : IStockNotReservedRequestEvent
    {
        public int OrderId { get; set; }
        public string Reason { get; set; }
    }
}
