namespace BeehiveManagementSystem
{
    internal interface IWorker
    {
        string Job { get; }

        bool WorkTheNextShift();

    }
}
