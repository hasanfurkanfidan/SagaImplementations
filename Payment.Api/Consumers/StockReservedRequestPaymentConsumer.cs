using MassTransit;
using Shared.Orchestiration.Events;
using Shared.Orchestiration.Interfaces;
using System.Threading.Tasks;

namespace Payment.Api.Consumers
{
    public class StockReservedRequestPaymentConsumer : IConsumer<IStockReservedRequestPayment>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        public StockReservedRequestPaymentConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<IStockReservedRequestPayment> context)
        {
            var balance = 3000m;
            if (balance > context.Message.Payment.TotalPrice)
            {
                await _publishEndpoint.Publish(new PaymentCompletedEvent(context.Message.CorrelationId));
            }
            else
            {
                await _publishEndpoint.Publish(new PaymentFailedEvent(context.Message.CorrelationId) { Reason = "not enough balance", OrderItems = context.Message.OrderItems });
            }
        }
    }
}
