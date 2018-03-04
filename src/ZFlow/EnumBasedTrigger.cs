using System;

namespace ZFlow
{
    public class EnumBasedActionsTrigger<TAction> where TAction : struct
    {
        public event EventHandler<EnumBasedActionEventArgs<TAction>> ActionCalled = (o, e) => { };

        public void OnActionCalled(TAction action)
        {
            this.ActionCalled(this, new EnumBasedActionEventArgs<TAction>(action));
        }
    }

    public class EnumBasedActionTrigger<TAction> : Trigger where TAction : struct
    {
        private readonly EnumBasedActionsTrigger<TAction> actionsTrigger;

        public TAction Action { get; private set; }

        public EnumBasedActionTrigger(EnumBasedActionsTrigger<TAction> actionsTrigger, TAction action)
        {
            this.actionsTrigger = actionsTrigger;
            this.Action = action;
        }

        public override void Initialize()
        {
            this.actionsTrigger.ActionCalled += this.OnActionCalled;
        }

        private void OnActionCalled(object sender, EnumBasedActionEventArgs<TAction> args)
        {
            if (this.Action.Equals(args.Action))
            {
                this.Pull();
            }
        }
    }

    public class EnumBasedActionEventArgs<TAction> : EventArgs
    {
        public TAction Action { get; private set; }

        public EnumBasedActionEventArgs(TAction action)
        {
            this.Action = action;
        }
    }
}