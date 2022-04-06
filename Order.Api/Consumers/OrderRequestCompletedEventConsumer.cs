using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Api.Models;
using Shared.Orchestiration.Interfaces;
using System.Threading.Tasks;

namespace Order.Api.Consumers
{
    public class OrderRequestCompletedEventConsumer : IConsumer<IOrderRequestCompletedEvent>
    {
        private readonly AppDbContext _appDbContext;
        public OrderRequestCompletedEventConsumer(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task Consume(ConsumeContext<IOrderRequestCompletedEvent> context)
        {
            var orderMessage = context.Message;
            var order = await _appDbContext.Order.SingleOrDefaultAsync(p => p.Id == orderMessage.OrderId);
            order.Status = OrderStatus.Success;
            await _appDbContext.SaveChangesAsync();
        }
    }
}
