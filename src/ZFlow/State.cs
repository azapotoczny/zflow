using System.Collections.Generic;

namespace ZFlow
{
    public class State
    {
        public string Name { get; set; }
        public bool IsFinal { get; set; }
        public Activity Entry { get; set; }
        public Activity Exit { get; set; }
        public List<Transition> Transitions { get; set; }

        public State(string name)
        {
            this.Name = name;
            this.IsFinal = false;
            this.Entry = new EmptyActivity();
            this.Exit = new EmptyActivity();
            this.Transitions = new List<Transition>();
        }

        public State WithEntryActivity(Activity activity)
        {
            this.Entry = activity;
            return this;
        }

        public State WithExitActivity(Activity activity)
        {
            this.Exit = activity;
            return this;
        }

        public State AddTransition(Transition transition)
        {
            this.Transitions.Add(transition);
            return this;
        }
    }
}