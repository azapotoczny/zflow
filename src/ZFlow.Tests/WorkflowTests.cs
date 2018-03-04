using Xunit;

namespace ZFlow.Tests
{
    public class WorkflowTests
    {
        [Fact]
        public void ShouldRunSimpleStateMachineWorkflow()
        {
            // Given
            var workflow = new Workflow()
                .AddState(new State("Initial")
                    .AddTransition(new Transition("Processing")))
                .AddState(new State("Processing")
                    .AddTransition(new Transition("Completed")))
                .AddState(new State("Completed"));

            // When
            workflow.Run();

            // Then
            Assert.NotNull(workflow.CurrentState);
            Assert.Equal("Completed", workflow.CurrentState.Name);
        }

        [Fact]
        public void ShouldStayInTheSameStateWhenConditionNotSatisfied()
        {
            // Given
            var workflow = new Workflow()
                .AddState(new State("Initial")
                    .AddTransition(new Transition("Completed").WithTrigger(new ConditionalTrigger(() => false))))
                .AddState(new State("Completed"));

            // When
            workflow.Run();

            // Then
            Assert.NotNull(workflow.CurrentState);
            Assert.Equal("Initial", workflow.CurrentState.Name);
        }
    }
}