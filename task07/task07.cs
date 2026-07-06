using System.ComponentModel;
using System.Reflection;
namespace task07;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class DisplayNameAttribute : Attribute
{
    public string DisplayName {get;}
    public DisplayNameAttribute(string name)
    {
        DisplayName = name;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class VersionAttribute : Attribute
{
    public int Major { get; }
    public int Minor { get; }

    public VersionAttribute(int major, int minor)
    {
        Major = major;
        Minor = minor;
    }
}

[DisplayName("Пример класса")]
[Version(1,0)]
public class SampleClass
{
    [DisplayName("Числовое свойство")]
    public int Number {get; set;}

    [DisplayName("Тестовый метод")]
    public void TestMethod()
    {
        
    }
}

public static class ReflectionHelper
{
    public static void PrintTypeInfo(Type type)
    {
        var output = new List<string>();

        var attrName = type.GetCustomAttribute<DisplayNameAttribute>();
        if (attrName!=null) Console.WriteLine(attrName.DisplayName);

        var attrVer = type.GetCustomAttribute<VersionAttribute>();
        if (attrVer!=null) Console.WriteLine($"{attrVer.Major}/{attrVer.Minor}");

        foreach (var prop in type.GetProperties())
        {
            var attrProp = prop.GetCustomAttribute<DisplayNameAttribute>();
            if (attrProp!=null) Console.WriteLine($"{prop.Name} - {attrProp.DisplayName}");
        }

        foreach (var method in type.GetMethods())
        {
            var attrMerhod = method.GetCustomAttribute<DisplayNameAttribute>();
            if (attrMerhod!=null) Console.WriteLine($"{method.Name} - {attrMerhod.DisplayName}");
        }
    }
}
