using Xunit;
using task17;

public class RecordingCommand : ICommand
{
    private readonly List<int> _log;
    private readonly int _id;
    public RecordingCommand(List<int> log, int id) { _log = log; _id = id; }
    public void Execute() { lock (_log) { _log.Add(_id); } }
}

public class SteppedRecordingCommand : ILongRunningCommand
{
    private readonly List<int> _log;
    private readonly int _id;
    private readonly int _totalSlices;
    private int _executedSlices;

    public SteppedRecordingCommand(List<int> log, int id, int totalSlices)
    {
        _log = log;
        _id = id;
        _totalSlices = totalSlices;
    }

    public bool IsCompleted => _executedSlices >= _totalSlices;

    public void Execute()
    {
        _executedSlices++;
        lock (_log) { _log.Add(_id); }
    }
}

public class SchedulerTests
{
    [Fact]
    public void LongRunningCommand_ExecutesSlicesUntilCompleted()
    {
        var log = new List<int>();
        var server = new ServerThread();

        server.Enqueue(new SteppedRecordingCommand(log, 1, totalSlices: 5));
        server.Enqueue(new SoftStop(server));
        server.Join();

        Assert.Equal(new[] { 1, 1, 1, 1, 1 }, log);
    }

    [Fact]
    public void TwoLongRunningCommands_AreInterleavedRoundRobin()
    {
        var log = new List<int>();
        var server = new ServerThread();

        server.Enqueue(new SteppedRecordingCommand(log, 1, totalSlices: 3));
        server.Enqueue(new SteppedRecordingCommand(log, 2, totalSlices: 3));
        server.Enqueue(new SoftStop(server));
        server.Join();

        Assert.Equal(new[] { 1, 2, 1, 2, 1, 2 }, log);
    }

    [Fact]
    public void ShortCommand_CompletesInOneSlice()
    {
        var log = new List<int>();
        var server = new ServerThread();

        server.Enqueue(new RecordingCommand(log, 42));
        server.Enqueue(new SoftStop(server));
        server.Join();

        Assert.Equal(new[] { 42 }, log);
    }

    [Fact]
    public void NewShortCommand_IsNotBlockedByLongRunningCommand()
    {
        var log = new List<int>();
        var server = new ServerThread();

        server.Enqueue(new SteppedRecordingCommand(log, 1, totalSlices: 10));
        server.Enqueue(new RecordingCommand(log, 99));
        server.Enqueue(new SoftStop(server));
        server.Join();

        Assert.Contains(99, log);
        int positionOf99 = log.IndexOf(99);
        int totalSlicesOf1BeforeThat = log.GetRange(0, positionOf99).FindAll(x => x == 1).Count;
        Assert.True(totalSlicesOf1BeforeThat < 10, "Короткая команда не должна ждать полного завершения длинной");
    }

    [Fact]
    public void HardStop_DiscardsUnfinishedLongRunningCommand()
    {
        var log = new List<int>();
        var server = new ServerThread();

        server.Enqueue(new SteppedRecordingCommand(log, 1, totalSlices: 100));
        server.Enqueue(new HardStop(server));
        server.Enqueue(new RecordingCommand(log, 2));

        server.Join();

        Assert.DoesNotContain(2, log);
        Assert.True(log.Count < 100);
    }

    [Fact]
    public void SoftStop_FinishLongRunningCommand()
    {
        var log = new List<int>();
        var server = new ServerThread();

        server.Enqueue(new SteppedRecordingCommand(log, 1, totalSlices: 4));
        server.Enqueue(new SoftStop(server));
        server.Join();

        Assert.Equal(4, log.Count);
    }

    [Fact]
    public void RoundRobinScheduler_SelectThrowsWhenEmpty()
    {
        var scheduler = new RoundRobinScheduler();
        Assert.False(scheduler.HasCommand());
        Assert.Throws<InvalidOperationException>(() => scheduler.Select());
    }
}
