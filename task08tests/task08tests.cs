using Commands;
using Xunit;

namespace task08tests;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDirSize");
        if (Directory.Exists(testDir)) Directory.Delete(testDir, true);
        Directory.CreateDirectory(testDir);
        
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello"); // 5 байт
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World"); // 5 байт

        var command = new DirectorySizeCommand(testDir);

        using var sw = new StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(sw);

        try
        {
            command.Execute();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        var output = sw.ToString();

        if (Directory.Exists(testDir)) Directory.Delete(testDir, true);

        Assert.Contains($"{testDir} - 10 байт", output);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDirFind");
        if (Directory.Exists(testDir)) Directory.Delete(testDir, true);
        Directory.CreateDirectory(testDir);
        
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        var command = new FindFilesCommand(testDir, "*.txt");

        using var sw = new StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(sw);

        try
        {
            command.Execute();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        var output = sw.ToString();

        if (Directory.Exists(testDir)) Directory.Delete(testDir, true);

        string expectedFilePath = Path.Combine(testDir, "file1.txt");

        Assert.Contains("Кол-во найденных файлов по маске: 1", output);
        Assert.Contains(expectedFilePath, output);
    }
}