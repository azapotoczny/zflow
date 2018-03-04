# ZFlow
ZFlow is a light weight state machine workflow targeting .NET Standard.


## Fluent API
Define your workflows with the fluent API.

## Real World Example

Lets consider order processing workflow. Here is a sample order class.

```c#
public class Order
{
    public string Number { get; set; }
    public Customer Customer { get; set; }
    public OrderStatus Status { get; set; }
}
```

And here is a complete workflow definition. It supports simple inventory check and notifications.

```c#
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
```

And here is how this workflow can be run.

```c#
var workflow = new OrderWorkflow(order);

workflow.Instance.Run();
workflow.OrderShippedTrigger.Pull();
workflow.OrderDeliveredTrigger.Pull();
```


