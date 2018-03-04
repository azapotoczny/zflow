using System;

namespace ZFlow
{
    public class Trigger
    {
        public event EventHandler Pulled = (o, e) => { };

        public void Pull()
        {
            this.Pulled(this, EventArgs.Empty);
        }

        public virtual void Initialize() { }
    }
}