using System;

namespace ZFlow.Sample
{
    public class OrderWorkflow
    {
        public Workflow Instance { get; }

        public Trigger OrderShippedTrigger { get; } = new Trigger();
        public Trigger OrderDeliveredTrigger { get; } = new Trigger();
        
        public OrderWorkflow(Order order)
        {
            this.Instance = new Workflow()
                .AddState(new State("New")
                    .AddTransition(new Transition("Processing")
                        .WithTrigger(new ConditionalTrigger(() => InventoryService.InStock(order))))
                    .AddTransition(new Transition("OnHold")
                        .WithTrigger(new InstantTrigger())))
                .AddState(new State("Processing")
                    .WithEntryActivity(new DelegateActivity(() => InventoryService.GetFromStock(order)))
                    .AddTransition(new Transition("Shipped")
                        .WithTrigger(this.OrderShippedTrigger)))
                .AddState(new State("Shipped")
                    .AddTransition(new Transition("Delivered")
                        .WithTrigger(this.OrderDeliveredTrigger)))
                .AddState(new State("Delivered"))
                .AddState(new State("OnHold"));

            this.Instance.StateChanged += (o, e) =>
            {
                order.Status = Enum.Parse<OrderStatus>(e.State.Name);
                
                NotificationService.Send(order.Customer.Email, 
                    $"Your order #{order.Number} has change status to '{order.Status}'.");
            };
        }
    }
}