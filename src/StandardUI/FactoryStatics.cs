using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Shapes;
using System;

namespace Microsoft.StandardUI
{
    public static class FactoryStatics
    {
        /*** UI Elements ***/

        public static ICanvas Canvas() => Factory.CreateCanvas();
        public static IStackPanel StackPanel() => Factory.CreateStackPanel();
        public static IGrid Grid() => Factory.CreateGrid();

        public static IEllipse Ellipse() => Factory.CreateEllipse();
        public static ILine Line() => Factory.CreateLine();
        public static IPath Path() => Factory.CreatePath();
        public static IPolygon Polygon() => Factory.CreatePolygon();
        public static IPolyline Polyline() => Factory.CreatePolyline();
        public static IRectangle Rectangle() => Factory.CreateRectangle();

        public static ITextBlock TextBlock() => Factory.CreateTextBlock();

        /*** Media objects ***/

        public static ISolidColorBrush SolidColorBrush() => Factory.CreateSolidColorBrush();
        public static ILinearGradientBrush LinearGradientBrush() => Factory.CreateLinearGradientBrush();
        public static IRadialGradientBrush RadialGradientBrush() => Factory.CreateRadialGradientBrush();

        /*** Environment peers ***/

        /*** Infrastructure objects ***/

        public static IUIPropertyMetadata UIPropertyMetdata(object defaultValue) => Factory.CreatePropertyMetadata(defaultValue);
        public static IUIPropertyMetadata UIPropertyMetdata(object defaultValue, UIPropertyChangedCallback propertyChangedCallback) => Factory.CreatePropertyMetadata(defaultValue, propertyChangedCallback);
        public static IUIProperty RegisterUIProperty(string name, Type propertyType, Type ownerType, IUIPropertyMetadata typeMetadata) =>
            Factory.RegisterUIProperty(name, propertyType, ownerType, typeMetadata);

        internal static IStandardUIFactory Factory => StandardUIEnvironment.Factory;
    }
}
