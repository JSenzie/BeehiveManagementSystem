namespace BeehiveManagementSystem
{
    internal abstract class Bee(string job)
    {
        public string Job { get; private set; } = job;

        protected abstract decimal CostPerShift { get; }

        public virtual bool WorkTheNextShift()
        {
            if (HoneyVault.ConsumeHoney(CostPerShift)) return true;
            return false;
        }
    }
}
