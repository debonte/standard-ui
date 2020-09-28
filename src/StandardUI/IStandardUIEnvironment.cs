namespace System.StandardUI
{
    public interface IStandardUIEnvironment
    {
        IVisualEnvironment VisualEnvironment { get; }
        IStandardUIFactory Factory { get; }
    }
}
