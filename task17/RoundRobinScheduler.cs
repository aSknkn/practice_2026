namespace task17
{
    public class RoundRobinScheduler : IScheduler
    {
        private readonly Queue<ICommand> _queue = new Queue<ICommand>();

        public bool HasCommand() => _queue.Count > 0;
        public ICommand Select() => _queue.Dequeue();
        public void Add(ICommand cmd) => _queue.Enqueue(cmd);
    }
}
