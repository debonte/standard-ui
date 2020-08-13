// This file is generated from IUIElement.cs. Update the source file to change its contents.

using Microsoft.StandardUI;
using System.Windows;

namespace Microsoft.StandardUI.Wpf
{
    public class UIElement : System.Windows.UIElement, IUIElement
    {
        public static readonly System.Windows.DependencyProperty WidthProperty = PropertyUtils.Create(nameof(Width), typeof(double), typeof(double), double.NaN);
        public static readonly System.Windows.DependencyProperty MinWidthProperty = PropertyUtils.Create(nameof(MinWidth), typeof(double), typeof(double), 0.0);
        public static readonly System.Windows.DependencyProperty MaxWidthProperty = PropertyUtils.Create(nameof(MaxWidth), typeof(double), typeof(double), double.PositiveInfinity);
        public static readonly System.Windows.DependencyProperty HeightProperty = PropertyUtils.Create(nameof(Height), typeof(double), typeof(double), double.NaN);
        public static readonly System.Windows.DependencyProperty MinHeightProperty = PropertyUtils.Create(nameof(MinHeight), typeof(double), typeof(double), 0.0);
        public static readonly System.Windows.DependencyProperty MaxHeightProperty = PropertyUtils.Create(nameof(MaxHeight), typeof(double), typeof(double), double.PositiveInfinity);
        
        public double Width
        {
            get => (double) GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }
        
        public double MinWidth
        {
            get => (double) GetValue(MinWidthProperty);
            set => SetValue(MinWidthProperty, value);
        }
        
        public double MaxWidth
        {
            get => (double) GetValue(MaxWidthProperty);
            set => SetValue(MaxWidthProperty, value);
        }
        
        public double Height
        {
            get => (double) GetValue(HeightProperty);
            set => SetValue(HeightProperty, value);
        }
        
        public double MinHeight
        {
            get => (double) GetValue(MinHeightProperty);
            set => SetValue(MinHeightProperty, value);
        }
        
        public double MaxHeight
        {
            get => (double) GetValue(MaxHeightProperty);
            set => SetValue(MaxHeightProperty, value);
        }
    }
}
