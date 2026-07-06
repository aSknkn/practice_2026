using System.Reflection;

namespace MetadataViewer;

class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Ошибка: Не указан путь к DLL.");
            return;
        }

        string dllPath = Path.GetFullPath(args[0]);

        if (!File.Exists(dllPath))
        {
            Console.WriteLine($"Ошибка: Файл не найден: {dllPath}");
            return;
        }

        try
        {
            Assembly assembly = Assembly.LoadFrom(dllPath);
            
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsClass) continue;

                Console.WriteLine($"\nКласс: {type.FullName}");

                foreach (var attr in type.GetCustomAttributes())
                {
                    Console.WriteLine($"  Атрибут класса: {attr.GetType().Name}");
                }

                foreach (var ctor in type.GetConstructors())
                {
                    Console.WriteLine($"  Конструктор: {ctor.Name}");
                    foreach (var p in ctor.GetParameters())
                    {
                        Console.WriteLine($"    Параметр: {p.Name} ({p.ParameterType.Name})");
                    }
                }

                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
                foreach (var method in methods)
                {
                    if (method.IsSpecialName) continue;

                    Console.WriteLine($"  Метод: {method.Name}");

                    foreach (var attr in method.GetCustomAttributes())
                    {
                        Console.WriteLine($"    Атрибут метода: {attr.GetType().Name}");
                    }

                    foreach (var p in method.GetParameters())
                    {
                        Console.WriteLine($"    Параметр: {p.Name} ({p.ParameterType.Name})");
                    }
                }
            }
        }
        catch (ReflectionTypeLoadException)
        {
            Console.WriteLine($"Ошибка загрузки типов");
        }
        catch (Exception)
        {
            Console.WriteLine($"Ошибка при работе с DLL");
        }
    }
}
