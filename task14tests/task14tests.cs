using Xunit;
using task14;

namespace task14tests;

public class DefiniteIntegralTests
{
    [Fact]
    public void Test_Xfunction_CorrectAnswer()
    {
        var X = (double x) => x;
        Assert.Equal(0, DefiniteIntegral.SolveMultiThread(-1, 1, X, 1e-4, 2), 1e-4);
        Assert.Equal(12.5, DefiniteIntegral.SolveMultiThread(0, 5, X, 1e-6, 8), 1e-5);
    }

    [Fact]
    public void Test_SINfunction_CorrectAnswer()
    {
        var SIN = (double x) => Math.Sin(x);
        Assert.Equal(0, DefiniteIntegral.SolveMultiThread(-1, 1, SIN, 1e-5, 8), 1e-4);
    }

    [Fact]
    public void Test_QUADfunction_CorrectAnswer()
    {
        var QUAD = (double x) => x * x;
        Assert.Equal(9.0, DefiniteIntegral.SolveMultiThread(0, 3, QUAD, 1e-4, 4), 3);
    }

    [Fact]
    public void Test_ZeroInterval_ReturnsZero()
    {
        var func = (double x) => Math.Pow(x, 3) - x + 5;
        Assert.Equal(0.0, DefiniteIntegral.SolveMultiThread(5, 5, func, 1e-4, 2), 4);
    }

    [Fact]
    public void Test_OddNumberOfThreads_CorrectRemainder()
    {
        var X = (double x) => x;
        Assert.Equal(50.0, DefiniteIntegral.SolveMultiThread(0, 10, X, 1.0, 3), 4);
    }

    [Fact]
    public void Test_ManyThreads_And_TinyStep()
    {
        var exp = Math.Exp;
        Assert.Equal(Math.E - 1, DefiniteIntegral.SolveMultiThread(0, 1, exp, 1e-6, 16), 4);
    }
}
