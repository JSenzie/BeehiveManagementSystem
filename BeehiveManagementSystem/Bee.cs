namespace BeehiveManagementSystem
{
    internal abstract class Bee(string job): IWorker
    {
        public string Job { get; } = job;

        protected abstract decimal CostPerShift { get; }

        public virtual bool WorkTheNextShift()
        {
            if (HoneyVault.ConsumeHoney(CostPerShift)) return true;
            return false;
        }
    }
}
