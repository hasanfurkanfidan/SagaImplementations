using MassTransit;
using System;

namespace Shared.Orchestiration.Interfaces
{
    public interface IPaymentCompletedEvent : CorrelatedBy<Guid>
    {
    }
}
