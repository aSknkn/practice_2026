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
        private readonly IScheduler _scheduler;

        public Thread WorkerThread => _thread;
        public bool IsAlive => _thread.IsAlive;

        public ServerThread(IScheduler scheduler = null, IExceptionHandler exceptionHandler = null)
        {
            _scheduler = scheduler ?? new RoundRobinScheduler();
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
        while (true)
         {
            if (!_scheduler.HasCommand())
            {
                if (!_queue.TryTake(out var awaited, Timeout.Infinite))
                    break;
                _scheduler.Add(awaited);
            }

            while (_queue.TryTake(out var newCmd, 0))
                _scheduler.Add(newCmd);

            var cmd = _scheduler.Select();

             try
             {
                 cmd.Execute();
                if (cmd is ILongRunningCommand lr && !lr.IsCompleted)
                    _scheduler.Add(cmd);
             }
             catch (Exception ex)
             {
                 _exceptionHandler?.Handle(ex, cmd);
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
