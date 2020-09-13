// This file is generated from ICanvas.cs. Update the source file to change its contents.

using System.StandardUI.Controls;

namespace System.StandardUI.Wpf.Controls
{
    public partial class Canvas : Panel, ICanvas
    {
        public static readonly Windows.DependencyProperty LeftProperty = PropertyUtils.RegisterAttached("Left", typeof(double), typeof(Windows.UIElement), 0.0);
        public static readonly Windows.DependencyProperty TopProperty = PropertyUtils.RegisterAttached("Top", typeof(double), typeof(Windows.UIElement), 0.0);
        
        public static double GetLeft(Windows.UIElement element) => (double) element.GetValue(LeftProperty);
        public static void SetLeft(Windows.UIElement element, double value) => element.SetValue(LeftProperty, value);
        
        public static double GetTop(Windows.UIElement element) => (double) element.GetValue(TopProperty);
        public static void SetTop(Windows.UIElement element, double value) => element.SetValue(TopProperty, value);
    }
}
