// This file is generated from ILinearGradientBrush.cs. Update the source file to change its contents.

using System.StandardUI.Media;
using System.Windows;

namespace System.StandardUI.Wpf.Media
{
    public class LinearGradientBrush : GradientBrush, ILinearGradientBrush
    {
        public static readonly Windows.DependencyProperty StartPointProperty = PropertyUtils.Register(nameof(StartPoint), typeof(PointWpf), typeof(LinearGradientBrush), PointWpf.Default);
        public static readonly Windows.DependencyProperty EndPointProperty = PropertyUtils.Register(nameof(EndPoint), typeof(PointWpf), typeof(LinearGradientBrush), PointWpf.Default);
        public PointWpf StartPoint
        {
            get => (PointWpf) GetValue(StartPointProperty);
            set => SetValue(StartPointProperty, value);
        }
        Point ILinearGradientBrush.StartPoint
        {
            get => StartPoint.Point;
            set => StartPoint = new PointWpf(value);
        }
        
        public PointWpf EndPoint
        {
            get => (PointWpf) GetValue(EndPointProperty);
            set => SetValue(EndPointProperty, value);
        }
        Point ILinearGradientBrush.EndPoint
        {
            get => EndPoint.Point;
            set => EndPoint = new PointWpf(value);
        }
    }
}
