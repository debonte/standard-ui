// This file is generated from IQuadraticBezierSegment.cs. Update the source file to change its contents.

using System.StandardUI.Media;

namespace System.StandardUI.Wpf.Media
{
    public class QuadraticBezierSegment : PathSegment, IQuadraticBezierSegment
    {
        public static readonly Windows.DependencyProperty Point1Property = PropertyUtils.Register(nameof(Point1), typeof(PointWpf), typeof(QuadraticBezierSegment), PointWpf.Default);
        public static readonly Windows.DependencyProperty Point2Property = PropertyUtils.Register(nameof(Point2), typeof(PointWpf), typeof(QuadraticBezierSegment), PointWpf.Default);
        
        public PointWpf Point1
        {
            get => (PointWpf) GetValue(Point1Property);
            set => SetValue(Point1Property, value);
        }
        Point IQuadraticBezierSegment.Point1
        {
            get => Point1.Point;
            set => Point1 = new PointWpf(value);
        }
        
        public PointWpf Point2
        {
            get => (PointWpf) GetValue(Point2Property);
            set => SetValue(Point2Property, value);
        }
        Point IQuadraticBezierSegment.Point2
        {
            get => Point2.Point;
            set => Point2 = new PointWpf(value);
        }
    }
}
