using System;

namespace ZFlow
{
    public class ConditionalTrigger : Trigger
    {
        private readonly Func<bool> condition;

        public ConditionalTrigger(Func<bool> condition)
        {
            this.condition = condition;
        }

        public override void Initialize()
        {
            if (this.condition.Invoke())
            {
                this.Pull();
            }
        }
    }
}