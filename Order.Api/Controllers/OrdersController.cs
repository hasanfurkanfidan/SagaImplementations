using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.Api.Dtos;
using Order.Api.Models;
using Shared.Constants;
using Shared.Orchestiration.Events;
using Shared.Orchestiration.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        public OrdersController(AppDbContext appDbContext, ISendEndpointProvider sendEndpointProvider)
        {
            _appDbContext = appDbContext;
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDto orderCreateDto)
        {
            var newOrder = new Models.Order
            {
                BuyerId = orderCreateDto.BuyerId,
                Status = OrderStatus.Waiting,
                CreatedDate = System.DateTime.Now,
                Adress = new Adress
                {
                    District = orderCreateDto.Adress.District,
                    Line = orderCreateDto.Adress.Line,
                    Province = orderCreateDto.Adress.Province
                },
            };
            orderCreateDto.OrderItems.ForEach(item =>
            {
                newOrder.OrderItems.Add(new OrderItem
                {
                    Count = item.Count,
                    ProductId = item.ProductId,
                    Price = item.Price
                });
            });
            await _appDbContext.AddAsync(newOrder);
            await _appDbContext.SaveChangesAsync();
            var orderCreatedRequestEvent = new OrderCreatedRequestEvent
            {
                BuyerId = orderCreateDto.BuyerId,
                OrderId = newOrder.Id,
                Payment = new Shared.EventMessages.PaymentMessage
                {
                    CardName = orderCreateDto.Payment.CardName,
                    CardNumber = orderCreateDto.Payment.CardNumber,
                    CVV = orderCreateDto.Payment.CVV,
                    TotalPrice = orderCreateDto.OrderItems.Sum(x => x.Price * x.Count),
                },
            };

            orderCreateDto.OrderItems.ForEach(orderCreateDto =>
            {
                orderCreatedRequestEvent.OrderItems.Add(new Shared.EventMessages.OrderItemMessage
                {
                    Count = orderCreateDto.Count,
                    ProductId = orderCreateDto.ProductId
                });
            });

            //await _publishEndpoint.Publish(orderCreatedEvent); (Caraographyden kalma)
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new System.Uri($"queue:{RabbitMqSettings.OrderSaga}"));
            await endpoint.Send<IOrderCreatedRequestEvent>(orderCreatedRequestEvent); // Send çünkü Sadece State Machine dinliyor!
            //Publish => Subscribe olan kuyruk yoksa boşa gider
            //Send => Direk Olarak Kuyruğa gider
            return Ok();
        }
    }
}
