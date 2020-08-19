using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI
{
    public interface IStandardUIEnvironment
    {
        IVisualEnvironment VisualEnvironment { get; }
        IUIElementFactory UIElementFactory { get; }
        IMediaFactory MediaFactory { get; }
    }
}
