using CommandLib;

namespace Plugins;

[PluginLoad("SecondPlugin")]
public class FirstPlugin : ICommand
{
    public void Execute()
    {
        Console.WriteLine(1);
    }
}

[PluginLoad("ThirdPlugin")]
public class SecondPlugin : ICommand
{
    public void Execute()
    {
        Console.WriteLine(2);
    }
}

[PluginLoad("")]
public class ThirdPlugin : ICommand
{
    public void Execute()
    {
        Console.WriteLine(3);
    }
}

[PluginLoad("")]
public class FourthPlugin : ICommand
{
    public void Execute()
    {
        Console.WriteLine(4);
    }
}
