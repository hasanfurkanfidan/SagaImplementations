using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Api.Models;
using Shared.Orchestiration.Interfaces;
using System.Threading.Tasks;

namespace Order.Api.Consumers
{
    public class OrderStockNotReservedEventConsumer : IConsumer<IStockNotReservedRequestEvent>
    {
        private readonly AppDbContext _appDbContext;
        public OrderStockNotReservedEventConsumer(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task Consume(ConsumeContext<IStockNotReservedRequestEvent> context)
        {
            var orderMessage = context.Message;
            var order = await _appDbContext.Order.SingleOrDefaultAsync(p => p.Id == orderMessage.OrderId);
            order.Status = OrderStatus.Fail;
            order.FailMessage = orderMessage.Reason;
            await _appDbContext.SaveChangesAsync();
        }
    }
}
