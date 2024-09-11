namespace BeehiveManagementSystem
{
    internal static class HoneyVault
    {
        private static decimal _honey = Constants.INITIAL_HONEY;
        private static decimal _nectar = Constants.INITIAL_NECTAR;

        internal static void Reset()
        {
            _honey = Constants.INITIAL_HONEY;
            _nectar = Constants.INITIAL_NECTAR;
        }

        public static string StatusReport {
            get 
            {
                string report = ($"Honey in vault: {_honey:0.00} units." +
                    $"\nNectar in vault: {_nectar:0.00} units.");


                if (_honey < Constants.LOW_LEVEL_WARNING)
                {
                    report += "\nLOW HONEY - ADD A MANUFACTURER.";
                }

                if (_nectar < Constants.LOW_LEVEL_WARNING)
                {
                    report += "\nLOW NECTAR - ADD A NECTAR COLLECTOR.";
                }

                return report;
            } 
        }

        /// <summary>
        /// Subtracts an amount of honey from the vault. Subtracts nothing if amount is greater than the available honey.
        /// </summary>
        /// <param name="amount">The amount of honey to subtract from the vault. </param>
        /// <returns>Whether the transaction occurred successfully.</returns>
        static public bool ConsumeHoney(decimal amount)
        {
            if(_honey >= amount)
            {
                _honey -= amount;
                return true;
            }
            return false;
        }


        /// <summary>
        /// Adds an amount of honey nectar to the vault.
        /// </summary>
        /// <param name="amount">The amount of nectar to add to the vault.</param>
        static public void CollectNectar(decimal amount) {
            if (amount > 0m) _nectar += amount;
        }


        /// <summary>
        /// Takes an amount of nectar and turns it into honey.
        /// </summary>
        /// <param name="amount">The amount of nectar to process into honey. If amount is more than the nectar left, all the remaining nectar will be used instead.</param>
        static public void ConvertNectarToHoney(decimal amount)
        {
            decimal nectarToConvert = amount;

            if (nectarToConvert > _nectar) nectarToConvert = _nectar;

            _nectar -= nectarToConvert;
            _honey += nectarToConvert * Constants.NECTAR_CONVERSION_RATIO;
        }
    }
}
