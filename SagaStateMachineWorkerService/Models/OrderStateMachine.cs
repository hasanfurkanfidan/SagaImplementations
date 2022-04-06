using Automatonymous;
using Shared.Constants;
using Shared.Orchestiration.Events;
using Shared.Orchestiration.Interfaces;
using System;

namespace SagaStateMachineWorkerService.Models
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }
        public Event<IStockReservedEvent> StockReservedEvent { get; set; }
        public Event<IPaymentCompletedEvent> PaymentCompletedEvent { get; set; }
        public Event<IStockNotReservedEvent> StockNotReservedEvent { get; set; }
        public Event<IPaymentFailedEvent> PaymentFailedEvent { get; set; }


        public State OrderCreated { get; set; }
        public State StockReserved { get; set; }
        public State PaymentCompleted { get; set; }
        public State StockNotReserved { get; set; }
        public State PaymentFailed { get; set; }
        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderCreatedRequestEvent, y => y.CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId).SelectId(context => System.Guid.NewGuid()));

            Initially(When(OrderCreatedRequestEvent).Then(context =>
            {
                context.Instance.BuyerId = context.Data.BuyerId;
                context.Instance.OrderId = context.Data.OrderId;
                context.Instance.CreatedDate = DateTime.Now;
                context.Instance.CardName = context.Data.Payment.CardName;
                context.Instance.CardNumber = context.Data.Payment.CardNumber;
                context.Instance.CVV = context.Data.Payment.CVV;
                context.Instance.Expiration = context.Data.Payment.Expiration;
                context.Instance.TotalPrice = context.Data.Payment.TotalPrice;
            })
                .Then(context => { Console.WriteLine($"OrderCreatedRequestEvent Before :{context.Instance} "); })
                .TransitionTo(OrderCreated)
                .Publish(context => new OrderCreatedEvent(context.Instance.CorrelationId)
                {
                    OrderItems = context.Data.OrderItems
                })
            .Then(context => { Console.WriteLine($"OrderCreatedRequestEvent After :{context.Instance.ToString()} "); }));

            During(OrderCreated, When(StockReservedEvent)
                .TransitionTo(StockReserved)
                .Send(new Uri($"queue:{RabbitMqSettings.PaymentStockReservedRequestQueueName}"), context => new StockReservedRequestPayment(context.Instance.CorrelationId)
                {
                    OrderItems = context.Data.OrderItems,
                    Payment = new Shared.EventMessages.PaymentMessage
                    {
                        CardName = context.Instance.CardName,
                        CardNumber = context.Instance.CardNumber,
                        CVV = context.Instance.CVV,
                        Expiration = context.Instance.Expiration,
                        TotalPrice = context.Instance.TotalPrice
                    }
                }).Then(context => { Console.WriteLine($"SrockReservedEvent After :{context.Instance} "); }),
                When(StockNotReservedEvent)
                .TransitionTo(StockNotReserved)
                .Send(new Uri($"queue:{RabbitMqSettings.OrderStockNotReservedRequestQueueName}"), context => new StockNotReservedRequestEvent
                {
                    OrderId = context.Instance.OrderId,
                    Reason = context.Data.Reason
                }));

            During(StockReserved, When(PaymentCompletedEvent)
                .TransitionTo(PaymentCompleted)
                .Publish(context => new OrderRequestCompletedEvent
                {
                    OrderId = context.Instance.OrderId,
                }).Finalize(), When(PaymentFailedEvent)
                .TransitionTo(PaymentFailed)
                .Publish(context => new PaymentFailedRequestEvent
                {
                    OrderId = context.Instance.OrderId,
                    OrderItems = context.Data.OrderItems,
                    Reason = context.Data.Reason
                }));


        }
    }
}
