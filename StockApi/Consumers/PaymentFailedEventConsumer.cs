using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Orchestiration.Interfaces;
using StockApi.Models;
using System.Threading.Tasks;

namespace StockApi.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<IPaymentFailedRequestEvent>
    {
        private readonly AppDbContext _appDbContext;
        public PaymentFailedEventConsumer(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task Consume(ConsumeContext<IPaymentFailedRequestEvent> context)
        {
            var message = context.Message;
            foreach (var orderItem in message.OrderItems)
            {
                var stock = await _appDbContext.Stocks.SingleOrDefaultAsync(p => p.ProductId == orderItem.ProductId);
                if (stock != null)
                {
                    stock.Count += orderItem.Count;
                }
            }
            await _appDbContext.SaveChangesAsync();
        }
    }
}
