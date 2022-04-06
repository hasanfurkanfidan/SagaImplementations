namespace Shared.Orchestiration.Interfaces
{
    public interface IStockNotReservedRequestEvent
    {
        public int OrderId { get; set; }
        public string Reason { get; set; }
    }
}
