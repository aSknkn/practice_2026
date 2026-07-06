using CommandLib;

namespace Commands;

public class DirectorySizeCommand : ICommand
{
    public string _path;
    public DirectorySizeCommand(string path)
    {
        _path = path;
    }

    public void Execute()
    {
        var directoryInfo = new DirectoryInfo(_path);
        
        long size = directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);
        
        Console.WriteLine($"{_path} - {size} байт");
    }
}

public class FindFilesCommand : ICommand
{
    private readonly string _path;
    private readonly string _mask;

    public FindFilesCommand(string path, string mask)
    {
        _path = path;
        _mask = mask;
    }

    public void Execute()
    {
        var files = Directory.GetFiles(_path, _mask);
        


        Console.WriteLine($"Кол-во найденных файлов по маске: {files.Length}");
        foreach (var file in files)
        {
            Console.WriteLine($"{file}");
        }
    }
}
