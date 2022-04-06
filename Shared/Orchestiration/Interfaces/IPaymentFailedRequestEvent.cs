using Shared.EventMessages;
using System.Collections.Generic;

namespace Shared.Orchestiration.Interfaces
{
    public interface IPaymentFailedRequestEvent
    {
        public int OrderId { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
        public string Reason { get; set; }
    }
}
