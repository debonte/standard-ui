namespace Microsoft.StandardUI
{
    public interface IStandardUIEnvironment
    {
        IVisualEnvironment VisualEnvironment { get; }
        IStandardUIFactory Factory { get; }
    }
}
