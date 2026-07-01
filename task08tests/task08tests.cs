namespace task08tests;
using Commands;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        var command = new DirectorySizeCommand(testDir);

        using var sw = new StringWriter();
        Console.SetOut(sw);

        command.Execute();
        
        var output = sw.ToString();

        Directory.Delete(testDir, true);
        Assert.Contains("/tmp/TestDir - 10 байт", output);

        
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        var command = new FindFilesCommand(testDir, "*.txt");

        using var sw = new StringWriter();
        Console.SetOut(sw);

        command.Execute();
        
        var output = sw.ToString();

        Directory.Delete(testDir, true);
        Assert.Contains("Кол-во найденных файлов по маске: 1", output);
        Assert.Contains("/tmp/TestDir/file1.txt", output);
    }
}