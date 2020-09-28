using System.StandardUI.Media;

namespace System.StandardUI.Wpf
{
    public class WpfStandardUIEnvironment : IStandardUIEnvironment
    {
        private StandardUIFactory _uiElementFactory = new StandardUIFactory();
        private IVisualEnvironment _visualEnvironment;

        public static void Init(IVisualEnvironment visualEnvironment)
        {
            StandardUIEnvironment.Init(new WpfStandardUIEnvironment(visualEnvironment));
        }

        public WpfStandardUIEnvironment(IVisualEnvironment visualEnvironment)
        {
            _visualEnvironment = visualEnvironment;
        }

        public IVisualEnvironment VisualEnvironment => _visualEnvironment;

        public IStandardUIFactory Factory => _uiElementFactory;
    }
}
