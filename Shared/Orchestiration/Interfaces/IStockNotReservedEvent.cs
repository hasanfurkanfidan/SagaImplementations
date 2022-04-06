using MassTransit;
using System;

namespace Shared.Orchestiration.Interfaces
{
    public interface IStockNotReservedEvent : CorrelatedBy<Guid>
    {
        public string Reason { get; set; }
    }
}
