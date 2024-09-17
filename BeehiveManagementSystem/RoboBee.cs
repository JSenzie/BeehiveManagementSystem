namespace BeehiveManagementSystem
{
    internal class RoboBee: Robot, IWorker
    {
        public string Job { get; set; } = "RoboBee";

        public bool WorkTheNextShift()
        {
            return true;
        }
    }
}
