namespace System.StandardUI
{
    public interface IVisualHostControl
    {
        IVisual? Content { set; }

        object? NativeControl { get; }
    }
}
