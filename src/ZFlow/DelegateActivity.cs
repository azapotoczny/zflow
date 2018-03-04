using System;

namespace ZFlow
{
    public class DelegateActivity : Activity
    {
        private readonly Action action;

        public DelegateActivity(Action action)
        {
            this.action = action;
        }

        public override void Execute()
        {
            this.action.Invoke();
        }
    }
}