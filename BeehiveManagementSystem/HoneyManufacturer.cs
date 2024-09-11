namespace BeehiveManagementSystem
{
    internal class HoneyManufacturer(): Bee("Honey Manufacturer")
    {
        protected override decimal CostPerShift => Constants.HONEY_MANUFACTURER_COST;

        public override bool WorkTheNextShift()
        {
            HoneyVault.ConvertNectarToHoney(Constants.NECTAR_PROCESSED_PER_SHIFT);
            return base.WorkTheNextShift();
        }
    }
}
