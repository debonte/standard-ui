// This file is generated from IGradientStop.cs. Update the source file to change its contents.

using System.StandardUI.Media;

namespace System.StandardUI.Wpf.Media
{
    public class GradientStop : Windows.DependencyObject, IGradientStop
    {
        public static readonly Windows.DependencyProperty ColorProperty = PropertyUtils.Register(nameof(Color), typeof(ColorWpf), typeof(GradientStop), ColorWpf.Default);
        public static readonly Windows.DependencyProperty OffsetProperty = PropertyUtils.Register(nameof(Offset), typeof(double), typeof(GradientStop), 0.0);
        
        public ColorWpf Color
        {
            get => (ColorWpf) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        Color IGradientStop.Color
        {
            get => Color.Color;
            set => Color = new ColorWpf(value);
        }
        
        public double Offset
        {
            get => (double) GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }
    }
}
