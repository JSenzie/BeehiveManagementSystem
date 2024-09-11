namespace BeehiveManagementSystem
{
    internal class EggCare: Bee
    {
        protected override decimal CostPerShift => Constants.EGG_CARE_COST;

        private Queen _queen;

        public EggCare(Queen queen): base("Egg Care")
        {
            _queen = queen;
        }

        public override bool WorkTheNextShift()
        {
            _queen.ReportEggConversion(Constants.CARE_PROGRESS_PER_SHIFT);
            return base.WorkTheNextShift();
        }
    }
}
