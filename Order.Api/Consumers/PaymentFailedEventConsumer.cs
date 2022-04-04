using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Api.Models;
using Shared.Event;
using System.Threading.Tasks;

namespace Order.Api.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly AppDbContext _appDbContext;
        public PaymentFailedEventConsumer(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var orderMessage = context.Message;
            var order = await _appDbContext.Order.SingleOrDefaultAsync(p => p.Id == orderMessage.OrderId);
            order.Status = OrderStatus.Fail;
            order.FailMessage = orderMessage.FailMessage;
            await _appDbContext.SaveChangesAsync();
        }
    }
}
