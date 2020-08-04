using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Shapes;

namespace Microsoft.StandardUI
{
    public static class FactoryExtensions
    {
        public static IEllipse Ellipse() => StandardUIEnvironment.UIElementFactory.CreateEllipse();

        public static ILine Line() => StandardUIEnvironment.UIElementFactory.CreateLine();

        public static IPath Path() => StandardUIEnvironment.UIElementFactory.CreatePath();

        public static IPolygon Polygon() => StandardUIEnvironment.UIElementFactory.CreatePolygon();

        public static IPolyline Polyline() => StandardUIEnvironment.UIElementFactory.CreatePolyline();

        public static IRectangle Rectangle() => StandardUIEnvironment.UIElementFactory.CreateRectangle();

        public static ICanvas Canvas() => StandardUIEnvironment.UIElementFactory.CreateCanvas();

        public static IStackPanel StackPanel() => StandardUIEnvironment.UIElementFactory.CreateStackPanel();

        public static IGrid Grid() => StandardUIEnvironment.UIElementFactory.CreateGrid();
    }
}
