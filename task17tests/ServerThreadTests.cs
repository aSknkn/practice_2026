using Xunit;
using task17;

public class RecordingCommand : ICommand
{
    private readonly List<int> _log;
    private readonly int _id;
    public RecordingCommand(List<int> log, int id) { _log = log; _id = id; }
    public void Execute() { lock (_log) { _log.Add(_id); } }
}

public class ServerThreadTests
{

    // TestExceptionHandler - тестовая реализация IExceptionHandler.
    // создана для сохранения ошибок в список Caught,
    // для тестирования коррекной передачи исключений
    private class TestExceptionHandler : IExceptionHandler
    {
        public List<Exception> Caught { get; } = new List<Exception>();
        public void Handle(Exception exception, ICommand command) => Caught.Add(exception);
    }


    [Fact]
    public void HardStop_DiscardsRemainingCommands()
    {
        var log = new List<int>();
        var server = new ServerThread();

        server.Enqueue(new RecordingCommand(log, 1));
        server.Enqueue(new RecordingCommand(log, 2));
        server.Enqueue(new HardStop(server));
        server.Enqueue(new RecordingCommand(log, 3));

        server.Join();

        Assert.Equal(new[] { 1, 2 }, log);
    }

    [Fact]
    public void SoftStop_DrainsQueueBeforeStopping()
    {
        var log = new List<int>();
        var server = new ServerThread();

        server.Enqueue(new RecordingCommand(log, 1));
        server.Enqueue(new RecordingCommand(log, 2));
        server.Enqueue(new SoftStop(server));
        server.Enqueue(new RecordingCommand(log, 3));
        server.Enqueue(new RecordingCommand(log, 4));

        server.Join();

        Assert.Equal(new[] { 1, 2, 3, 4 }, log);
        Assert.Throws<InvalidOperationException>(() => server.Enqueue(new RecordingCommand(log, 5)));
    }

    [Fact]
    public void HardStop_ThrowsWhenExecutedOutsideTargetThread()
    {
        var serverA = new ServerThread();
        var hardStopForA = new HardStop(serverA);

        Assert.Throws<InvalidOperationException>(() => hardStopForA.Execute());

        serverA.Enqueue(new HardStop(serverA));
        serverA.Join();
    }

    [Fact]
    public void HardStop_TargetingWrongServer_IsCaughtByExceptionHandler()
    {
        var handler = new TestExceptionHandler();

        var serverA = new ServerThread();
        var serverB = new ServerThread(handler);

        serverB.Enqueue(new HardStop(serverA));
        serverB.Enqueue(new SoftStop(serverB));
        serverB.Join();

        Assert.Single(handler.Caught);
        Assert.IsType<InvalidOperationException>(handler.Caught[0]);
    }


    [Fact]
    public void HardStop_WrongThread_ExceptionMessageIsCorrect()
    {
        var server = new ServerThread();
        var hardStop = new HardStop(server);

        var ex = Assert.Throws<InvalidOperationException>(() => hardStop.Execute());
        Assert.Equal("HardStop должен выполняться в потоке, который он останавливает.", ex.Message);

        server.Enqueue(new HardStop(server));
        server.Join();
    }

    [Fact]
    public void SoftStop_WrongThread_ExceptionMessageIsCorrect()
    {
        var server = new ServerThread();
        var softStop = new SoftStop(server);

        var ex = Assert.Throws<InvalidOperationException>(() => softStop.Execute());
        Assert.Equal("SoftStop должен выполняться в потоке, который он останавливает.", ex.Message);

        server.Enqueue(new SoftStop(server));
        server.Join();
    }
}
