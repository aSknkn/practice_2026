using System.Reflection;

namespace MetadataViewer;

class Program
{
    public static void Main(string[] args)
    {
        string dllPath = args[0];
        Assembly assembly = Assembly.LoadFrom(dllPath);

        foreach (var type in assembly.GetTypes())
        {
            Console.WriteLine($"\nКласс: {type.Name}");

            foreach (var attr in type.GetCustomAttributes())
                Console.WriteLine($"  Атрибут: {attr}");

            foreach (var ctor in type.GetConstructors())
            {
                Console.WriteLine($"  Конструктор: {ctor}");
                foreach (var p in ctor.GetParameters())
                    Console.WriteLine($"    {p.Name} ({p.ParameterType.Name})");
            }

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                Console.WriteLine($"  Метод: {method.Name}");
                foreach (var p in method.GetParameters())
                    Console.WriteLine($"    {p.Name} ({p.ParameterType.Name})");
            }
        }
    }
}