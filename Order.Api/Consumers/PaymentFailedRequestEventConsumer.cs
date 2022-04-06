using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Api.Models;
using Shared.Orchestiration.Events;
using System.Threading.Tasks;

namespace Order.Api.Consumers
{
    public class PaymentFailedRequestEventConsumer : IConsumer<PaymentFailedRequestEvent>
    {
        private readonly AppDbContext _appDbContext;
        public PaymentFailedRequestEventConsumer(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task Consume(ConsumeContext<PaymentFailedRequestEvent> context)
        {
            var orderMessage = context.Message;
            var order = await _appDbContext.Order.SingleOrDefaultAsync(p => p.Id == orderMessage.OrderId);
            order.Status = OrderStatus.Fail;
            order.FailMessage = orderMessage.Reason;
            await _appDbContext.SaveChangesAsync();
        }
    }
}
