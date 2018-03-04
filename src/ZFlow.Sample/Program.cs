using System;

namespace ZFlow.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var order = new Order
            {
                Number = "123",
                Customer = new Customer
                {
                    Name = "John Doe",
                    Email = "john@doe.com"
                }
            };
            
                var workflow = new OrderWorkflow(order);
                
                workflow.Instance.Run();
                workflow.OrderShippedTrigger.Pull();
                workflow.OrderDeliveredTrigger.Pull();
        }
    }
}