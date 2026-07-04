using Xunit;
using task11;

namespace task11tests;

public class CalculatorTests
{

    [Fact]
    public void Calculator_ReturnCorrectSolve()
    {
        ICalculator calc = CalculatorGenerator.CreateCalculatorFromString(
        @"
            public class Calculator{
                public int Add(int a, int b) => a + b;
                public int Minus(int a, int b) => a - b;
                public int Mul(int a, int b) => a * b;
                public int Div(int a, int b) => a / b;
            }");

        Assert.Equal(4, calc.Add(2, 2));
        Assert.Equal(0, calc.Minus(2, 2));
        Assert.Equal(4, calc.Mul(2, 2));
        Assert.Equal(1, calc.Div(2, 2));
    }
}
