using System;

namespace ZFlow.Sample
{
    public class Order
    {
        public string Number { get; set; }
        public Customer Customer { get; set; }
        public OrderStatus Status { get; set; }
    }
}