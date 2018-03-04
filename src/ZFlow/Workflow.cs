using System;
using System.Collections.Generic;
using System.Linq;

namespace ZFlow
{
    public class Workflow
    {
        public event EventHandler<WorkflowStateChangedEventArgs> StateChanged = (o, e) => { };

        public IList<State> States { get; private set; }

        public State CurrentState { get; private set; }

        public bool IsSusspended { get; private set; }

        public Workflow()
        {
            this.States = new List<State>();
            this.IsSusspended = false;
        }

        public Workflow AddState(State state)
        {
            this.States.Add(state);
            return this;
        }

        public void Validate()
        {
            if (this.States.Count == 0)
            {
                throw new InvalidOperationException("Workflow must have at least one state.");
            }

            foreach (var transition in this.States.SelectMany(x => x.Transitions))
            {
                var targetState = this.GetState(transition.Target);
                if (targetState == null)
                {
                    throw new InvalidOperationException($"Target state '{transition.Target}' is not defined.");
                }
            }
        }

        public void Restore(string currentState)
        {
            this.CurrentState = this.GetState(currentState);
        }

        public void Run()
        {
            this.Validate();

            if (this.CurrentState == null)
            {
                this.CurrentState = this.States.First();
                this.CurrentState.Entry.Execute();

                this.OnStateChanged();
            }

            this.AttachToTriggers(this.CurrentState);
        }

        public void Suspend()
        {
            this.IsSusspended = true;
        }

        public void Resume()
        {
            this.IsSusspended = false;
        }

        private void AttachToTriggers(State state)
        {
            foreach (var trigger in state.Transitions.Select(x => x.Trigger))
            {
                trigger.Pulled += this.OnTriggerPulled;
            }

            foreach (var trigger in state.Transitions.Select(x => x.Trigger))
            {
                trigger.Initialize();
            }
        }

        private void DetachFromTriggers(State state)
        {
            foreach (var trigger in state.Transitions.Select(x => x.Trigger))
            {
                trigger.Pulled -= this.OnTriggerPulled;
            }
        }

        private void OnTriggerPulled(object sender, EventArgs args)
        {
            var transition = this.CurrentState
                .Transitions.Single(x => x.Trigger == sender);

            this.PerformTransition(transition);
        }

        private void PerformTransition(Transition transition)
        {
            if (this.IsSusspended)
            {
                throw new InvalidOperationException("Unable to perform transition, the workflow is susspended.");
            }

            transition.Action.Execute();

            this.DetachFromTriggers(this.CurrentState);

            this.CurrentState.Exit.Execute();
            this.CurrentState = this.GetState(transition.Target);
            this.CurrentState.Entry.Execute();

            this.OnStateChanged();

            this.AttachToTriggers(this.CurrentState);
        }

        public State GetState(string name)
        {
            return this.States.FirstOrDefault(x => x.Name == name);
        }

        private void OnStateChanged()
        {
            this.StateChanged(this, new WorkflowStateChangedEventArgs(this, this.CurrentState));
        }
    }

    public class WorkflowStateChangedEventArgs : EventArgs
    {
        public Workflow Workflow { get; private set; }
        public State State { get; private set; }

        public WorkflowStateChangedEventArgs(Workflow workflow, State state)
        {
            this.Workflow = workflow;
            this.State = state;
        }
    }
}