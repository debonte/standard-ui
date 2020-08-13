// This file is generated from IGradientStop.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using System.Windows;

namespace Microsoft.StandardUI.Wpf.Media
{
    public class GradientStop : System.Windows.DependencyObject, IGradientStop
    {
        public static readonly System.Windows.DependencyProperty ColorProperty = PropertyUtils.Create(nameof(Color), typeof(ColorWpf), typeof(ColorWpf), ColorWpf.Default);
        public static readonly System.Windows.DependencyProperty OffsetProperty = PropertyUtils.Create(nameof(Offset), typeof(double), typeof(double), 0.0);
        
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
