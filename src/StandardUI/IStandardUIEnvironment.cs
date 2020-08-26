using System.StandardUI.Media;

namespace System.StandardUI
{
    public interface IStandardUIEnvironment
    {
        IVisualEnvironment VisualEnvironment { get; }
        IUIElementFactory UIElementFactory { get; }
        IMediaFactory MediaFactory { get; }
    }
}
