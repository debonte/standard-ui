// This file is generated from IArcSegment.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using System.Windows;

namespace Microsoft.StandardUI.Wpf.Media
{
    public class ArcSegment : PathSegment, IArcSegment
    {
        public static readonly System.Windows.DependencyProperty PointProperty = PropertyUtils.Create(nameof(Point), typeof(PointWpf), typeof(PointWpf), PointWpf.Default);
        public static readonly System.Windows.DependencyProperty SizeProperty = PropertyUtils.Create(nameof(Size), typeof(SizeWpf), typeof(SizeWpf), SizeWpf.Default);
        public static readonly System.Windows.DependencyProperty RotationAngleProperty = PropertyUtils.Create(nameof(RotationAngle), typeof(double), typeof(double), 0.0);
        public static readonly System.Windows.DependencyProperty IsLargeArcProperty = PropertyUtils.Create(nameof(IsLargeArc), typeof(bool), typeof(bool), false);
        public static readonly System.Windows.DependencyProperty SweepDirectionProperty = PropertyUtils.Create(nameof(SweepDirection), typeof(SweepDirection), typeof(SweepDirection), SweepDirection.Counterclockwise);
        
        public PointWpf Point
        {
            get => (PointWpf) GetValue(PointProperty);
            set => SetValue(PointProperty, value);
        }
        Point IArcSegment.Point
        {
            get => Point.Point;
            set => Point = new PointWpf(value);
        }
        
        public SizeWpf Size
        {
            get => (SizeWpf) GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }
        Size IArcSegment.Size
        {
            get => Size.Size;
            set => Size = new SizeWpf(value);
        }
        
        public double RotationAngle
        {
            get => (double) GetValue(RotationAngleProperty);
            set => SetValue(RotationAngleProperty, value);
        }
        
        public bool IsLargeArc
        {
            get => (bool) GetValue(IsLargeArcProperty);
            set => SetValue(IsLargeArcProperty, value);
        }
        
        public SweepDirection SweepDirection
        {
            get => (SweepDirection) GetValue(SweepDirectionProperty);
            set => SetValue(SweepDirectionProperty, value);
        }
    }
}
