using Xunit;
using task07;
using System.Reflection;

public class EmptySampleClass
{
}

public class AttributeReflectionTests
{
    [Fact]
    public void Class_HasDisplayNameAttribute()
    {
        var type = typeof(SampleClass);
        var attribute = type.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Пример класса", attribute.DisplayName);
    }

    [Fact]
    public void Method_HasDisplayNameAttribute()
    {
        var method = typeof(SampleClass).GetMethod("TestMethod");
        var attribute = method.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Тестовый метод", attribute.DisplayName);
    }

    [Fact]
    public void Property_HasDisplayNameAttribute()
    {
        var prop = typeof(SampleClass).GetProperty("Number");
        var attribute = prop.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Числовое свойство", attribute.DisplayName);
    }

    [Fact]
    public void Class_HasVersionAttribute()
    {
        var type = typeof(SampleClass);
        var attribute = type.GetCustomAttribute<VersionAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Major);
        Assert.Equal(0, attribute.Minor);
    }

    [Fact]
    public void ReflectionHelper_CorrectOutput()
    {
        using var sw = new StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(sw);

        try
        {
            ReflectionHelper.PrintTypeInfo(typeof(SampleClass));
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        string output = sw.ToString();

        Assert.Contains("Пример класса", output);
        Assert.Contains("1/0", output);
        Assert.Contains("Number - Числовое свойство", output);
        Assert.Contains("TestMethod - Тестовый метод", output);
    }

    [Fact]
    public void ReflectionHelper_EmptyOutput_WhenTypeHasNoCustomAttributes()
    {
        using var sw = new StringWriter();
        var originalOut = Console.Out;
        Console.SetOut(sw);

        try
        {
            ReflectionHelper.PrintTypeInfo(typeof(EmptySampleClass));
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        string output = sw.ToString();

        Assert.Empty(output);
    }
}
