namespace Shared.Orchestiration.Interfaces
{
    public interface IOrderRequestCompletedEvent
    {
        public int OrderId { get; set; }
    }
}
