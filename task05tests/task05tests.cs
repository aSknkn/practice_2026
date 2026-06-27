using Xunit;
using task05;

namespace task05tests;

public class TestClass
{
    public int PublicField;
    private string _privateField;
    public int Property { get; set; }

    public void Method(int arg1, int arg2) { }
}

[Serializable]
public class AttributedClass { }

public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Contains("Method", methods);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
    }

    [Fact]
    public void GetProperties_ReturnsCorrectProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties();

        Assert.Contains("Property", properties);
    }

    [Fact]
    public void GetMethodParams_ReturnsCorrectSignature()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var MethodParams = analyzer.GetMethodParams("Method");

        Assert.Contains("arg1", MethodParams);
        Assert.Contains("arg2", MethodParams);
        Assert.Contains("Void", MethodParams);
    }

    [Fact]
    public void HasAttribute_ReturnsTrue()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        bool hasAttribute = analyzer.HasAttribute<SerializableAttribute>();

        Assert.True(hasAttribute);
    }

    [Fact]
    public void HasAttribute_ReturnsFalse()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        bool hasAttribute = analyzer.HasAttribute<SerializableAttribute>();

        Assert.False(hasAttribute);
    }
}
