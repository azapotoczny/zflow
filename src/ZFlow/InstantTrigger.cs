namespace ZFlow
{
    public class InstantTrigger : Trigger
    {
        public override void Initialize()
        {
            this.Pull();
        }
    }
}