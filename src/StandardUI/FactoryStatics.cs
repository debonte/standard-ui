using System.StandardUI.Controls;
using System.StandardUI.Media;
using System.StandardUI.Shapes;

namespace System.StandardUI
{
    public static class FactoryStatics
    {
        public static ICanvas Canvas() => StandardUIEnvironment.Factory.CreateCanvas();
        public static IStackPanel StackPanel() => StandardUIEnvironment.Factory.CreateStackPanel();
        public static IGrid Grid() => StandardUIEnvironment.Factory.CreateGrid();

        public static IEllipse Ellipse() => StandardUIEnvironment.Factory.CreateEllipse();
        public static ILine Line() => StandardUIEnvironment.Factory.CreateLine();
        public static IPath Path() => StandardUIEnvironment.Factory.CreatePath();
        public static IPolygon Polygon() => StandardUIEnvironment.Factory.CreatePolygon();
        public static IPolyline Polyline() => StandardUIEnvironment.Factory.CreatePolyline();
        public static IRectangle Rectangle() => StandardUIEnvironment.Factory.CreateRectangle();

        public static ITextBlock TextBlock() => StandardUIEnvironment.Factory.CreateTextBlock();

        public static ISolidColorBrush SolidColorBrush() => StandardUIEnvironment.Factory.CreateSolidColorBrush();
        public static ILinearGradientBrush LinearGradientBrush() => StandardUIEnvironment.Factory.CreateLinearGradientBrush();
        public static IRadialGradientBrush RadialGradientBrush() => StandardUIEnvironment.Factory.CreateRadialGradientBrush();
    }
}
