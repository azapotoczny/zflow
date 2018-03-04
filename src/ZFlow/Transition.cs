namespace ZFlow
{
    public class Transition
    {
        public Trigger Trigger { get; set; }
        public Activity Action { get; set; }
        public string Target { get; set; }

        public Transition(string target)
        {
            this.Trigger = new InstantTrigger();
            this.Action = new EmptyActivity();
            this.Target = target;
        }

        public Transition WithTrigger(Trigger trigger)
        {
            this.Trigger = trigger;
            return this;
        }
    }
}