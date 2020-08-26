using System.StandardUI.Controls;
using System.StandardUI.Shapes;
using System.StandardUI.Wpf.Controls;
using System.StandardUI.Wpf.Shapes;

namespace System.StandardUI.Wpf
{
    public class UIElementFactory : IUIElementFactory
    {
        public ICanvas CreateCanvas() => new Canvas();
        public ICanvasAttached CanvasAttached => Controls.CanvasAttached.Instance;

        public IEllipse CreateEllipse() => new Ellipse();
        public IGrid CreateGrid() => new Grid();
        public ILine CreateLine() => new Line();
        public IPath CreatePath() => new Path();
        public IPolygon CreatePolygon() => new Polygon();
        public IPolyline CreatePolyline() => new Polyline();
        public IRectangle CreateRectangle() => new Rectangle();
        public IStackPanel CreateStackPanel() => new StackPanel();
    }
}
