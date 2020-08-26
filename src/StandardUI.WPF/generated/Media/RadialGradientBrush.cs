// This file is generated from IRadialGradientBrush.cs. Update the source file to change its contents.

using System.StandardUI.Media;
using System.Windows;

namespace System.StandardUI.Wpf.Media
{
    public class RadialGradientBrush : GradientBrush, IRadialGradientBrush
    {
        public static readonly System.Windows.DependencyProperty CenterProperty = PropertyUtils.Register(nameof(Center), typeof(PointWpf), typeof(RadialGradientBrush), PointWpf.CenterDefault);
        public static readonly System.Windows.DependencyProperty GradientOriginProperty = PropertyUtils.Register(nameof(GradientOrigin), typeof(PointWpf), typeof(RadialGradientBrush), PointWpf.CenterDefault);
        public static readonly System.Windows.DependencyProperty RadiusXProperty = PropertyUtils.Register(nameof(RadiusX), typeof(double), typeof(RadialGradientBrush), 0.5);
        
        public PointWpf Center
        {
            get => (PointWpf) GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }
        Point IRadialGradientBrush.Center
        {
            get => Center.Point;
            set => Center = new PointWpf(value);
        }
        
        public PointWpf GradientOrigin
        {
            get => (PointWpf) GetValue(GradientOriginProperty);
            set => SetValue(GradientOriginProperty, value);
        }
        Point IRadialGradientBrush.GradientOrigin
        {
            get => GradientOrigin.Point;
            set => GradientOrigin = new PointWpf(value);
        }
        
        public double RadiusX
        {
            get => (double) GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }
    }
}
