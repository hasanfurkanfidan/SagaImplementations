using System;
using System.Collections.Generic;

namespace Order.Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BuyerId { get; set; }
        public string FailMessage { get; set; }
        public OrderStatus Status { get; set; }
        public Adress Adress { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
    public enum OrderStatus
    {
        Waiting,
        Success,
        Fail
    }
}
