using MassTransit;
using Shared.Event;
using StockApi.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StockApi.Consumers
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
            foreach (var item in context.Message.OrderItemMessages)
            {
                var stock = _appDbContext.Stocks.Where(p => p.ProductId == item.ProductId).FirstOrDefault();
                stock.Count += item.Count;
            }
            await _appDbContext.SaveChangesAsync();
        }
    }
}
