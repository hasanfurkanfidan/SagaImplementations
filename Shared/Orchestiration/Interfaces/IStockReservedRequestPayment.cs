using MassTransit;
using Shared.EventMessages;
using System;
using System.Collections.Generic;

namespace Shared.Orchestiration.Interfaces
{
    public interface IStockReservedRequestPayment : CorrelatedBy<Guid>
    {
        public PaymentMessage Payment { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
