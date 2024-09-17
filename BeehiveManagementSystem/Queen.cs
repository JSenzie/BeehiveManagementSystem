using System.ComponentModel;

namespace BeehiveManagementSystem
{
    internal class Queen: Bee, INotifyPropertyChanged
    {

        private IWorker[] _workers = new IWorker[0];
        private decimal _unassignedWorkers = 3M;
        private decimal _eggs = 0M;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool HiveIsRunning { get; private set; } = true;
        public bool OutOfHoney { get { return !HiveIsRunning; } }


        public string StatusReport { get; private set; } = "";

        protected override decimal CostPerShift => Constants.QUEEN_COST_PER_SHIFT;

        public bool CanAssignWorkers => _unassignedWorkers >=1;

        public Queen(): base("Queen")
        {
            AssignBee("Egg Care");
            AssignBee("Nectar Collector");
            AssignBee("Honey Manufacturer");
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool WorkTheNextShift()
        {
            _eggs += Constants.EGGS_PER_SHIFT;

            bool allWorkersCompletedShifts = true;
            foreach (IWorker worker in _workers)
            {
                if (!worker.WorkTheNextShift())
                {
                    allWorkersCompletedShifts = false;
                }
            }

            HoneyVault.ConsumeHoney(Constants.HONEY_PER_UNASSIGNED_WORKER * _unassignedWorkers);

            bool shiftWorked = base.WorkTheNextShift();
            if (!shiftWorked) HiveIsRunning = false;
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

        private void AddWorker(IWorker worker)
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
            OnPropertyChanged(nameof(StatusReport));
            OnPropertyChanged(nameof(CanAssignWorkers));
            OnPropertyChanged(nameof(HiveIsRunning));
            OnPropertyChanged(nameof(OutOfHoney));

        }

        private string WorkerStatus(string jobType)
        {
            int count = 0;
            foreach (IWorker worker in _workers) {
                if (worker.Job == jobType) count++;
            }
            string s = "s";
            if (count == 1) s = "";

            return $"{count} {jobType} bee" + s;
        }
    }
}
