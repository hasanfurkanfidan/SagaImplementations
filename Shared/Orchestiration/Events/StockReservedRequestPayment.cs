using Shared.EventMessages;
using Shared.Orchestiration.Interfaces;
using System;
using System.Collections.Generic;

namespace Shared.Orchestiration.Events
{
    public class StockReservedRequestPayment : IStockReservedRequestPayment
    {
        public StockReservedRequestPayment(Guid corralationId)
        {
            CorrelationId = corralationId;
        }
        public PaymentMessage Payment { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }

        public Guid CorrelationId { get; }
    }
}
