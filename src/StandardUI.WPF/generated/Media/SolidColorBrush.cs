// This file is generated from ISolidColorBrush.cs. Update the source file to change its contents.

using System.StandardUI.Media;
using System.Windows;

namespace System.StandardUI.Wpf.Media
{
    public class SolidColorBrush : Brush, ISolidColorBrush
    {
        public static readonly Windows.DependencyProperty ColorProperty = PropertyUtils.Register(nameof(Color), typeof(ColorWpf), typeof(SolidColorBrush), ColorWpf.Default);
        public ColorWpf Color
        {
            get => (ColorWpf) GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        Color ISolidColorBrush.Color
        {
            get => Color.Color;
            set => Color = new ColorWpf(value);
        }
    }
}
