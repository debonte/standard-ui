// This file is generated from ILineSegment.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using System.Windows;

namespace Microsoft.StandardUI.Wpf.Media
{
    public class LineSegment : PathSegment, ILineSegment
    {
        public static readonly System.Windows.DependencyProperty PointProperty = PropertyUtils.Create(nameof(Point), typeof(PointWpf), typeof(PointWpf), PointWpf.Default);
        
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
