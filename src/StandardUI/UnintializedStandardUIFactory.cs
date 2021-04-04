using Microsoft.StandardUI.Media;
using System;
using Microsoft.StandardUI.Controls;
using Microsoft.StandardUI.Shapes;
using System.Collections.Generic;

namespace Microsoft.StandardUI
{
    public class UnintializedStandardUIFactory : IStandardUIFactory
    {
        public static UnintializedStandardUIFactory Instance = new UnintializedStandardUIFactory();

        /*** UI Elements ***/

        public ICanvas CreateCanvas() => throw CreateInitNotCalledException();
        public ICanvasAttached CanvasAttached => throw CreateInitNotCalledException();
        public IStackPanel CreateStackPanel() => throw CreateInitNotCalledException();
        public IGrid CreateGrid() => throw CreateInitNotCalledException();

        public IEllipse CreateEllipse() => throw CreateInitNotCalledException();
        public ILine CreateLine() => throw CreateInitNotCalledException();
        public IPath CreatePath() => throw CreateInitNotCalledException();
        public IPolygon CreatePolygon() => throw CreateInitNotCalledException();
        public IPolyline CreatePolyline() => throw CreateInitNotCalledException();
        public IRectangle CreateRectangle() => throw CreateInitNotCalledException();

        public ITextBlock CreateTextBlock() => throw CreateInitNotCalledException();

        /*** Media objects ***/

        public ISolidColorBrush CreateSolidColorBrush() => throw CreateInitNotCalledException();
        public ILinearGradientBrush CreateLinearGradientBrush() => throw CreateInitNotCalledException();
        public IRadialGradientBrush CreateRadialGradientBrush() => throw CreateInitNotCalledException();

        public ILineSegment CreateLineSegment(in Point point) => throw CreateInitNotCalledException();
        public IPolyLineSegment CreatePolyLineSegment(Points points) => throw CreateInitNotCalledException();
        public IBezierSegment CreateBezierSegment(in Point point1, in Point point2, in Point point3) => throw CreateInitNotCalledException();
        public IPolyBezierSegment CreatePolyBezierSegment(Points points) => throw CreateInitNotCalledException();
        public IQuadraticBezierSegment CreateQuadraticBezierSegment(in Point point1, in Point point2) => throw CreateInitNotCalledException();
        public IPolyQuadraticBezierSegment CreatePolyQuadraticBezierSegment(Points points) => throw CreateInitNotCalledException();
        public IArcSegment CreateArcSegment(in Point point, in Size size, double rotationAngle, bool isLargeArc,
            SweepDirection sweepDirection) => throw CreateInitNotCalledException();

        public IPathGeometry CreatePathGeometry(ITransform? transform, IEnumerable<IPathFigure> figures, FillRule fillRule) => throw CreateInitNotCalledException();
        public IPathFigure CreatePathFigure(IEnumerable<IPathSegment> segments, Point startPoint, bool isClosed, bool isFilled) => throw CreateInitNotCalledException();

        /*** Environment peers ***/

        public IStandardUIControlEnvironmentPeer CreateStandardUIControlEnvironmentPeer(StandardUIControl standardUIControl) =>
            throw CreateInitNotCalledException();

        /*** Infrastructure objects ***/

        public IUIPropertyMetadata CreatePropertyMetadata(object defaultValue) => throw CreateInitNotCalledException();
        public IUIPropertyMetadata CreatePropertyMetadata(object defaultValue, UIPropertyChangedCallback propertyChangedCallback) => throw CreateInitNotCalledException();
        public IUIProperty RegisterUIProperty(string name, Type propertyType, Type ownerType, IUIPropertyMetadata typeMetadata) => throw CreateInitNotCalledException();

        private Exception CreateInitNotCalledException() => new InvalidOperationException("StandardUIEnvironment.Init hasn't been called");
    }
}
