using System.Reflection;
using CommandLib;

namespace CommandRunner;

class Program
{
    static void Main()
    {
        string dllPath = "/home/artem/учёбаб/practice_2026/FileSystemCommands/bin/Debug/net8.0/FileSystemCommands.dll";

        Assembly assembly = Assembly.LoadFrom(dllPath);

        var commandTypes = assembly.GetTypes().Where(type => typeof(ICommand).IsAssignableFrom(type) && type.IsClass).ToList();

        string testDirectory = Environment.CurrentDirectory;
        string testMask = "*.*"; 

        foreach (var type in commandTypes)
        {
            ICommand command = null;
            if (type.Name == "DirectorySizeCommand")
            {
            command = (ICommand)Activator.CreateInstance(type, testDirectory);
            }
            else if (type.Name == "FindFilesCommand")
            {
            command = (ICommand)Activator.CreateInstance(type, testDirectory, testMask);
            }

            command?.Execute();
        }
    }
}
