using Xunit;
using PluginSystem;

namespace PluginSystemTests;

public class ConsoleTests
{
    [Fact]
    public void Test_MainMethodOutput()
    {
        var originalOut = Console.Out;
        using var sw = new StringWriter();
        Console.SetOut(sw);
        Program.Main();
            
        string output = sw.ToString();

        Assert.Contains("3", output);
        Assert.Contains("2", output);
        Assert.Contains("1", output);
        Assert.Contains("4", output);
    }
}
