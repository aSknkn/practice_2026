namespace CommandLib;

public interface ICommand
{
    void Execute();
}


[AttributeUsage(AttributeTargets.Class)]
public class PluginLoadAttribute : Attribute
{
    public string PluginLoadPastNode {get;}
    public PluginLoadAttribute(string pastName)
    {
        PluginLoadPastNode = pastName;
    }
}
