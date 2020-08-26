// This file is generated from ILineSegment.cs. Update the source file to change its contents.

using System.StandardUI.Media;
using System.Windows;

namespace System.StandardUI.Wpf.Media
{
    public class LineSegment : PathSegment, ILineSegment
    {
        public static readonly Windows.DependencyProperty PointProperty = PropertyUtils.Register(nameof(Point), typeof(PointWpf), typeof(LineSegment), PointWpf.Default);
        public PointWpf Point
        {
            get => (PointWpf) GetValue(PointProperty);
            set => SetValue(PointProperty, value);
        }
        Point ILineSegment.Point
        {
            get => Point.Point;
            set => Point = new PointWpf(value);
        }
    }
}
