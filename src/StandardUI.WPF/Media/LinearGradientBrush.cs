// This file is generated from ILinearGradientBrush.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using System.Windows;

namespace Microsoft.StandardUI.Wpf.Media
{
    public class LinearGradientBrush : GradientBrush, ILinearGradientBrush
    {
        public static readonly System.Windows.DependencyProperty StartPointProperty = PropertyUtils.Create(nameof(StartPoint), typeof(PointWpf), typeof(PointWpf), PointWpf.Default);
        public static readonly System.Windows.DependencyProperty EndPointProperty = PropertyUtils.Create(nameof(EndPoint), typeof(PointWpf), typeof(PointWpf), PointWpf.Default);
        
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
