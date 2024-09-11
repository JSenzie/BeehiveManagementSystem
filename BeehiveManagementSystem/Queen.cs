namespace BeehiveManagementSystem
{
    internal class Queen: Bee
    {

        private Bee[] _workers = new Bee[0];
        private decimal _unassignedWorkers = 3M;
        private decimal _eggs = 0M;
        public string StatusReport { get; private set; } = "";

        protected override decimal CostPerShift => Constants.QUEEN_COST_PER_SHIFT;

        public bool CanAssignWorkers => _unassignedWorkers >=1;

        public Queen(): base("Queen")
        {
            AssignBee("Egg Care");
            AssignBee("Nectar Collector");
            AssignBee("Honey Manufacturer");
        }

        public override bool WorkTheNextShift()
        {
            _eggs += Constants.EGGS_PER_SHIFT;

            bool allWorkersCompletedShifts = true;
            foreach (Bee worker in _workers)
            {
                if (!worker.WorkTheNextShift())
                {
                    allWorkersCompletedShifts = false;
                }
            }

            HoneyVault.ConsumeHoney(Constants.HONEY_PER_UNASSIGNED_WORKER * _unassignedWorkers);

            bool shiftWorked = base.WorkTheNextShift();
            UpdateStatusReport(allWorkersCompletedShifts);
            return shiftWorked;
        }

        public void AssignBee(string? jobType)
        {
            switch (jobType) 
            {
                case "Egg Care":
                    AddWorker(new EggCare(this));
                    break;
                case "Honey Manufacturer":
                    AddWorker(new HoneyManufacturer());
                    break;
                case "Nectar Collector":
                    AddWorker(new NectarCollector());
                    break;
            }
            UpdateStatusReport(true);
        }

        public void ReportEggConversion(decimal eggsToConvert)
        {
            if (_eggs >= eggsToConvert)
            {
                _eggs -= eggsToConvert;
                _unassignedWorkers += eggsToConvert;
            }
        }

        private void AddWorker(Bee worker)
        {
            if(_unassignedWorkers >= 1){
                _unassignedWorkers--;
                Array.Resize(ref _workers, _workers.Length + 1);
                _workers[_workers.Length - 1] = worker;
            }
        }

        private void UpdateStatusReport(bool allWorkersCompletedShifts)
        {
            StatusReport = $"Vault report:\n{HoneyVault.StatusReport}" +
                $"\nEgg Count: {_eggs:0.00}\nUnassigned Workers: {_unassignedWorkers:0.00}\n" +
                $"{WorkerStatus("Nectar Collector")}\n{WorkerStatus("Honey Manufacturer")}\n" +
                $"{WorkerStatus("Egg Care")}\nTOTAL WORKERS: {_workers.Length}";

            if (!allWorkersCompletedShifts) StatusReport += "\nWARNING: NOT ALL WORKERS WERE ABLE TO COMPLETE THEIR SHIFTS";
        }

        private string WorkerStatus(string jobType)
        {
            int count = 0;
            foreach (Bee worker in _workers) {
                if (worker.Job == jobType) count++;
            }
            string s = "s";
            if (count == 1) s = "";

            return $"{count} {jobType} bee" + s;
        }
    }
}
