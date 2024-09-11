namespace BeehiveManagementSystem
{
    internal class NectarCollector(): Bee("Nectar Collector")
    {
        protected override decimal CostPerShift => Constants.NECTAR_COLLECTOR_COST;

        public override bool WorkTheNextShift()
        {
            HoneyVault.CollectNectar(Constants.NECTAR_COLLECTED_PER_SHIFT);
            return base.WorkTheNextShift();
        }

    }
}
