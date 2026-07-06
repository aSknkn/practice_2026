using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace task11;

public interface ICalculator
{
    public int Add(int a, int b);
    public int Minus(int a, int b);
    public int Mul(int a, int b);
    public int Div(int a, int b);
}

public static class CalculatorGenerator
{
    public static ICalculator CreateCalculatorFromString(string rawCode)
    {
        int openIndex = rawCode.IndexOf('{');
        string modifiedCode = rawCode.Insert(openIndex, " : task11.ICalculator ");

        var syntaxTree = CSharpSyntaxTree.ParseText(modifiedCode);

        var references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
            MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location)
        };

        var compilation = CSharpCompilation.Create(
            Path.GetRandomFileName(),
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using (var ms = new MemoryStream())
        {
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                throw new InvalidOperationException("Ошибка компиляции");
            }

            ms.Seek(0, SeekOrigin.Begin);
            Assembly assembly = Assembly.Load(ms.ToArray());
            
            Type type = assembly.GetType("Calculator");
            
            return (ICalculator)Activator.CreateInstance(type);
        }
    }
}
