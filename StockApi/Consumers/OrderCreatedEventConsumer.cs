using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.Event;
using StockApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockApi.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly AppDbContext _appDbContext;
        private ILogger<OrderCreatedEventConsumer> _logger;
        private readonly ISendEndpointProvider _sendEnpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;
        public OrderCreatedEventConsumer(AppDbContext appDbContext, ILogger<OrderCreatedEventConsumer> logger, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _sendEnpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var stockResult = new List<bool>();

            foreach (var item in context.Message.OrderItems)
            {
                stockResult.Add(await _appDbContext.Stocks.AnyAsync(p => p.ProductId == item.ProductId && p.Count > item.Count));
            }

            if (stockResult.All(x => x.Equals(true)))
            {
                foreach (var orderItemMessage in context.Message.OrderItems)
                {
                    var stock = await _appDbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == orderItemMessage.ProductId);
                    if (stock != null)
                    {
                        stock.Count -= orderItemMessage.Count;
                    }
                    await _appDbContext.SaveChangesAsync();
                }
                _logger.LogInformation($"Sotock Reserved Successfully for BuyerId : {context.Message.BuyerId}");

                var sendEndpoint = await _sendEnpointProvider.GetSendEndpoint(new System.Uri($"queue:{RabbitMqSettings.StockReservedEventQueueName}"));

                var stockReservedEvent = new StockReservedEvent
                {
                    Payment = context.Message.Payment,
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.OrderItems
                };

                await sendEndpoint.Send(stockReservedEvent);
            }
            else
            {
                await _publishEndpoint.Publish(new StockNotReservedEvent
                {
                    OrderId = context.Message.OrderId,
                    FailMessage = "Stock Not Reserved Successfully"
                });
            }
        }
    }
}
