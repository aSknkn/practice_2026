using System.Collections.Concurrent;

namespace task17
{
    public interface ICommand
    {
        void Execute();
    }
    public interface IExceptionHandler
    {
        void Handle(Exception exception, ICommand command);
    }


    public class ServerThread
    {
        private readonly BlockingCollection<ICommand> _queue = new BlockingCollection<ICommand>();
        private readonly Thread _thread;
        private readonly IExceptionHandler _exceptionHandler;
        private volatile bool _hardStopRequested;

        public Thread WorkerThread => _thread;
        public bool IsAlive => _thread.IsAlive;

        public ServerThread(IExceptionHandler exceptionHandler = null)
        {
            _exceptionHandler = exceptionHandler;
            _thread = new Thread(Run) { IsBackground = true };
            _thread.Start();
        }

        public void Enqueue(ICommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            try
            {
                _queue.Add(command);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Поток не принимает команды: ", ex);
            }
        }

        private void Run()
        {
            foreach (var command in _queue.GetConsumingEnumerable())
            {
                try
                {
                    command.Execute();
                }
                catch (Exception ex)
                {
                    _exceptionHandler?.Handle(ex, command);
                }

                if (_hardStopRequested)
                    break;
            }
        }

        internal void RequestHardStop()
        {
            _hardStopRequested = true;
            _queue.CompleteAdding();
        }

        internal void RequestSoftStop()
        {
            _queue.CompleteAdding();
        }

        public void Join() => _thread.Join();
    }

    public class HardStop : ICommand
    {
        private readonly ServerThread _target;

        public HardStop(ServerThread target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public void Execute()
        {
            if (Thread.CurrentThread != _target.WorkerThread)
                throw new InvalidOperationException("HardStop должен выполняться в потоке, который он останавливает.");
            _target.RequestHardStop();
        }
    }

    public class SoftStop : ICommand
    {
        private readonly ServerThread _target;

        public SoftStop(ServerThread target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public void Execute()
        {
            if (Thread.CurrentThread != _target.WorkerThread)
                throw new InvalidOperationException("SoftStop должен выполняться в потоке, который он останавливает.");
            _target.RequestSoftStop();
        }
    }
}
