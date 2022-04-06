using Automatonymous;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace SagaStateMachineWorkerService.Models
{
    public class OrderStateInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; } //Her bir satır için random id (Gelen event hangi satırla ilgili ??)
        public string CurrentState { get; set; }
        public string BuyerId { get; set; }
        public int OrderId { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public override string ToString()
        {
            var properties = GetType().GetProperties();
            var stringBuilder = new StringBuilder();
            properties.ToList().ForEach(p =>
            {
                var value = p.GetValue(this, null);
                stringBuilder.AppendLine($"{p.Name} : {value}");
            });
            stringBuilder.AppendLine("-------------------");
            return stringBuilder.ToString();
        }
    }
}
