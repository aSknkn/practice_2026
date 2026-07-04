using System.Reflection;
using CommandLib;

namespace PluginSystem;

class Program
{
    static List<Type> allPluginTypes = new List<Type>();
    static List<Type> sortedPlugins = new List<Type>();
    static HashSet<string> visited = new HashSet<string>();

    static void SortPlugins(Type type)
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
        string dllPath = "/home/artem/учёбаб/practice_2026/Plugins/bin/Debug/net8.0/Plugins.dll";
        Assembly assembly = Assembly.LoadFrom(dllPath);

        foreach (var type in assembly.GetTypes())
        {
            if (type.IsClass)
            {
                allPluginTypes.Add(type);
            }
        }

        foreach (var type in allPluginTypes)
        {
            SortPlugins(type);
        }

        foreach (var type in sortedPlugins)
        {
            var instance = (ICommand)Activator.CreateInstance(type);
            instance.Execute();
        }
    }
}
