using Microsoft.StandardUI.Media;
using Microsoft.StandardUI.Wpf.Media;
using System.Collections.Generic;

namespace Microsoft.StandardUI.Wpf
{
    public class MediaFactory : IMediaFactory
    {
        public ISolidColorBrush CreateSolidColorBrush() => new SolidColorBrush();
        public ILinearGradientBrush CreateLinearGradientBrush() => new LinearGradientBrush();
        public IRadialGradientBrush CreateRadialGradientBrush() => new RadialGradientBrush();

        public IArcSegment CreateArcSegment(in Point point, in Size size, double rotationAngle, bool isLargeArc, SweepDirection sweepDirection)
        {
            throw new System.NotImplementedException();
        }

        public IBezierSegment CreateBezierSegment(in Point point1, in Point point2, in Point point3)
        {
            throw new System.NotImplementedException();
        }

        public ILineSegment CreateLineSegment(in Point point)
        {
            throw new System.NotImplementedException();
        }

        public IPathFigure CreatePathFigure(IEnumerable<IPathSegment> segments, Point startPoint, bool isClosed, bool isFilled)
        {
            throw new System.NotImplementedException();
        }

        public IPathGeometry CreatePathGeometry(ITransform? transform, IEnumerable<IPathFigure> figures, FillRule fillRule)
        {
            throw new System.NotImplementedException();
        }

        public IPolyBezierSegment CreatePolyBezierSegment(Points points)
        {
            throw new System.NotImplementedException();
        }

        public IPolyLineSegment CreatePolyLineSegment(Points points)
        {
            throw new System.NotImplementedException();
        }

        public IPolyQuadraticBezierSegment CreatePolyQuadraticBezierSegment(Points points)
        {
            throw new System.NotImplementedException();
        }

        public IQuadraticBezierSegment CreateQuadraticBezierSegment(in Point point1, in Point point2)
        {
            throw new System.NotImplementedException();
        }
    }
}
