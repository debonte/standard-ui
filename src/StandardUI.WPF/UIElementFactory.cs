using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Shapes;
using Microsoft.StandardUI.Wpf.Controls;
using Microsoft.StandardUI.Wpf.Shapes;

namespace Microsoft.StandardUI.Wpf
{
    public class UIElementFactory : IUIElementFactory
    {
        public ICanvas CreateCanvas() => new Canvas();
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
