// This file is generated from ISolidColorBrush.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Media;
using System.Windows;

namespace Microsoft.StandardUI.Wpf.Media
{
    public class SolidColorBrush : Brush, ISolidColorBrush
    {
        public static readonly System.Windows.DependencyProperty ColorProperty = PropertyUtils.Create(nameof(Color), typeof(ColorWpf), typeof(ColorWpf), ColorWpf.Default);
        
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
