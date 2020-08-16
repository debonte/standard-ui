using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI
{
    public interface IStandardUIEnvironment
    {
        IUIElementFactory UIElementFactory { get; }
        IMediaFactory MediaFactory { get; }
    }
}
