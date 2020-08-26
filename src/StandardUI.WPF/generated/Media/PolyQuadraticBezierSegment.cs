// This file is generated from IPolyQuadraticBezierSegment.cs. Update the source file to change its contents.

using System.StandardUI.Media;
using System.Windows;

namespace System.StandardUI.Wpf.Media
{
    public class PolyQuadraticBezierSegment : PathSegment, IPolyQuadraticBezierSegment
    {
        public static readonly Windows.DependencyProperty PointsProperty = PropertyUtils.Register(nameof(Points), typeof(PointsWpf), typeof(PolyQuadraticBezierSegment), PointsWpf.Default);
        public PointsWpf Points
        {
            get => (PointsWpf) GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
        Points IPolyQuadraticBezierSegment.Points
        {
            get => Points.Points;
            set => Points = new PointsWpf(value);
        }
    }
}
