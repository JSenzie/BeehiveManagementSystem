namespace BeehiveManagementSystem
{
    public partial class MainPage : ContentPage
    {
        private Queen _queen = new();

        public MainPage()
        {
            InitializeComponent();
            JobPicker.ItemsSource = new string[]
            { 
                "Nectar Collector",
                "Honey Manufacturer",
                "Egg Care",
            };
            JobPicker.SelectedIndex = 0;

            //UpdateStatusAndEnableAssignButton();
            Dispatcher.StartTimer(TimeSpan.FromSeconds(1.5), TimerTick);
            BindingContext = _queen;
        }

        private bool TimerTick()
        {
            if  (!this.IsLoaded || !WorkShiftButton.IsVisible)
            {
                return false;
            }
            WorkShiftButton_Clicked(this, new EventArgs());
            return true;
        }

        //private void UpdateStatusAndEnableAssignButton()
        //{
        //    StatusReport.Text = _queen.StatusReport;
        //    AssignJobButton.IsEnabled = (_queen.CanAssignWorkers);
        //}

        private void AssignJobButton_Clicked(object sender, EventArgs e)
        {
            _queen.AssignBee(JobPicker.SelectedItem.ToString());
            //UpdateStatusAndEnableAssignButton();
        }

        private void WorkShiftButton_Clicked(object sender, EventArgs e)
        {
            if (!_queen.WorkTheNextShift()){
                //WorkShiftButton.IsVisible = false;
                //OutOfHoneyButton.IsVisible = true;
                SemanticScreenReader.Default.Announce(OutOfHoneyButton.Text);
            }
            //UpdateStatusAndEnableAssignButton();
            SemanticScreenReader.Announce(_queen.StatusReport);
        }

        private void OutOfHoneyButton_Clicked(object sender, EventArgs e)
        {
            HoneyVault.Reset();
            _queen = new();
            BindingContext = _queen;
            //WorkShiftButton.IsVisible = true;
            //OutOfHoneyButton.IsVisible = false;
            //UpdateStatusAndEnableAssignButton();
            Dispatcher.StartTimer(TimeSpan.FromSeconds(1.5), TimerTick);

        }
    }

}
