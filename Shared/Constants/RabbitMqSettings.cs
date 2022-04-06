namespace Shared.Constants
{
    public class RabbitMqSettings
    {
        public const string OrderSaga = "order-saga-queue";

        public const string StockOrderCreatedEventQueueName = "stock-order-created-queue";
        public const string StockReservedEventQueueName = "stock-reserved-event-queue";
        public const string OrderPaymentCompletedEventQueueName = "order-payment-completed-queue";
        public const string OrderPaymentFailedEventQueueName = "order-payment-failed-queue";
        public const string OrderStockNotReservedEventQueueName = "order-stock-not-reserved-queue";
        public const string StockPaymentFailedEventQueueName = "stock-payment-failed-queue";

        public const string PaymentStockReservedRequestQueueName = "payment-stock-reserved-request-queue";
        public const string OrderRequestCompletedEventtQueueName = "order-completed-queue";
        public const string OrderStockNotReservedRequestQueueName = "order-stock-not-reserved-request-queue";
        public const string OrderPaymentFailedRequestQueue = "order-payment-failed-request-queue";
        public const string StockPaymentFailedRequestQueue = "stock-payment-failed-request-queue";
    }
}
