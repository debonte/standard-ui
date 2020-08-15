// This file is generated from IUIElement.cs. Update the source file to change its contents.

using Microsoft.StandardUI;
using System.Windows;

namespace Microsoft.StandardUI.Wpf
{
    public class UIElement : System.Windows.UIElement, IUIElement
    {
        public static readonly System.Windows.DependencyProperty WidthProperty = PropertyUtils.Register(nameof(Width), typeof(double), typeof(UIElement), double.NaN);
        public static readonly System.Windows.DependencyProperty MinWidthProperty = PropertyUtils.Register(nameof(MinWidth), typeof(double), typeof(UIElement), 0.0);
        public static readonly System.Windows.DependencyProperty MaxWidthProperty = PropertyUtils.Register(nameof(MaxWidth), typeof(double), typeof(UIElement), double.PositiveInfinity);
        public static readonly System.Windows.DependencyProperty HeightProperty = PropertyUtils.Register(nameof(Height), typeof(double), typeof(UIElement), double.NaN);
        public static readonly System.Windows.DependencyProperty MinHeightProperty = PropertyUtils.Register(nameof(MinHeight), typeof(double), typeof(UIElement), 0.0);
        public static readonly System.Windows.DependencyProperty MaxHeightProperty = PropertyUtils.Register(nameof(MaxHeight), typeof(double), typeof(UIElement), double.PositiveInfinity);
        
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
