using System.Reflection;
using CommandLib;

namespace PluginSystem;

public class Program
{
public static List<Type> allPluginTypes = new List<Type>();
    public static List<Type> sortedPlugins = new List<Type>();
    public static HashSet<string> visited = new HashSet<string>();

    public static void SortPlugins(Type type)
    {
        if (visited.Contains(type.Name)) return;

        var attribute = type.GetCustomAttribute<PluginLoadAttribute>();
        string pastName = attribute.PluginLoadPastNode;

        if (pastName != "")
        {
            foreach (var plugin in allPluginTypes)
            {
                if (plugin.Name == pastName)
                {
                    SortPlugins(plugin);
                    break;
                }
            }
        }

        sortedPlugins.Add(type);
        visited.Add(type.Name);
    }

    public static void Main()
    {
        string[] dllFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.dll");

        foreach (var dllPath in dllFiles)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(dllPath);
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsClass && !type.IsAbstract && typeof(ICommand).IsAssignableFrom(type))
                    {
                        allPluginTypes.Add(type);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Ошибка загрузки плагина {Path.GetFileName(dllPath)}");
            }
        }

        foreach (var type in allPluginTypes)
        {
            SortPlugins(type);
        }

        foreach (var type in sortedPlugins)
        {
            try
            {
                var instance = (ICommand)Activator.CreateInstance(type);
                instance?.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Ошибка выполнения плагина {type.Name}]: {ex.Message}");
            }
        }
    }
}
