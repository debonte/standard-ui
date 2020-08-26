using System.Windows.Media;

namespace System.StandardUI.SkiaVisualizer
{
    public class WpfNativeVisual : IVisual
    {
        public DrawingGroup DrawingGroup { get; }

        public WpfNativeVisual(DrawingGroup drawingGroup)
        {
            DrawingGroup = drawingGroup;
        }

        public object NativeVisual => DrawingGroup;
    }
}
