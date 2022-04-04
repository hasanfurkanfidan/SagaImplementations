using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Event;
using System.Threading.Tasks;

namespace Payment.Api.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly ILogger<StockReservedEventConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint, ILogger<StockReservedEventConsumer> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var balance = 3000m;
            if (balance > context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} tl was withdrawn from credik card for user {context.Message.BuyerId}");

                await _publishEndpoint.Publish(new PaymentCompletedEvent
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                });
            }
            else
            {
                _logger.LogInformation($"{context.Message.Payment} was not withdrawn from credit card for user: {context.Message.BuyerId}");
                await _publishEndpoint.Publish(new PaymentFailedEvent { OrderId = context.Message.OrderId, FailMessage = "not enough balance", OrderItemMessages = context.Message.OrderItems });
            }
        }
    }
}
