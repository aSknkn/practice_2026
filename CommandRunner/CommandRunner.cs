using System.Reflection;
using CommandLib;

namespace CommandRunner;

class Program
{
    static void Main()
    {
        string dllPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../FileSystemCommands/bin/Debug/net8.0/FileSystemCommands.dll"));

        Assembly assembly;
        try
        {
            if (!File.Exists(dllPath))
            {
                throw new Exception($"Файл плагина не найден по пути: {dllPath}");
            }
            assembly = Assembly.LoadFrom(dllPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки DLL");
            return;
        }

        var commandTypes = assembly.GetTypes()
            .Where(type => typeof(ICommand).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
            .ToList();

        string testDirectory = Environment.CurrentDirectory;
        string testMask = "*.*"; 

        foreach (var type in commandTypes)
        {
            var ctor = type.GetConstructors().FirstOrDefault();
            if (ctor == null) continue;

            var parameters = ctor.GetParameters();
            object[] ctorArgs = new object[parameters.Length];

            if (parameters.Length >= 1) ctorArgs[0] = testDirectory;
            if (parameters.Length >= 2) ctorArgs[1] = testMask;

            var command = (ICommand)Activator.CreateInstance(type, ctorArgs);        
            command?.Execute();
        }
    }
}