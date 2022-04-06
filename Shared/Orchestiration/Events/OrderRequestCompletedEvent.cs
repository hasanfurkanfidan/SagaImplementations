using Shared.Orchestiration.Interfaces;

namespace Shared.Orchestiration.Events
{
    public class OrderRequestCompletedEvent : IOrderRequestCompletedEvent
    {
        public int OrderId { get; set; }
    }
}
