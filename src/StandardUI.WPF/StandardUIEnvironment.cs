using Microsoft.StandardUI.Media;

namespace Microsoft.StandardUI.Wpf
{
    public class WpfStandardUIEnvironment : IStandardUIEnvironment
    {
        private UIElementFactory _uiElementFactory = new UIElementFactory();
        private MediaFactory _mediaFactory = new MediaFactory();
        private IVisualEnvironment _visualEnvironment;

        public static void Init(IVisualEnvironment visualEnvironment)
        {
            StandardUIEnvironment.Init(new WpfStandardUIEnvironment(visualEnvironment));
        }

        public WpfStandardUIEnvironment(IVisualEnvironment visualEnvironment)
        {
            _visualEnvironment = visualEnvironment;
        }

        public IUIElementFactory UIElementFactory => _uiElementFactory;

        public IMediaFactory MediaFactory => _mediaFactory;

        public IVisualEnvironment VisualEnvironment => _visualEnvironment;
    }
}
