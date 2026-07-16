namespace task17
{
    public class TestCommand : ICommand, ILongRunningCommand
    {
        private readonly int _id;
        private readonly int _maxCalls;
        private readonly CountdownEvent _completionSignal;
        private int _counter = 0;

        public TestCommand(int id, int maxCalls = 3, CountdownEvent completionSignal = null)
        {
            _id = id;
            _maxCalls = maxCalls;
            _completionSignal = completionSignal;
        }

        public bool IsCompleted => _counter >= _maxCalls;

        public void Execute()
        {
            Console.WriteLine($"Поток {_id} вызов {++_counter}");

            if (IsCompleted)
                _completionSignal?.Signal();
        }
    }
}
