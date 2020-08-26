// This file is generated from ICanvas.cs. Update the source file to change its contents.

using System.StandardUI.Controls;
using System.Windows;

namespace System.StandardUI.Wpf.Controls
{
    public class Canvas : Panel, ICanvas
    {
        public static readonly Windows.DependencyProperty LeftProperty = PropertyUtils.RegisterAttached("Left", typeof(double), typeof(UIElement), 0.0);
        public static readonly Windows.DependencyProperty TopProperty = PropertyUtils.RegisterAttached("Top", typeof(double), typeof(UIElement), 0.0);
        
        public static double GetLeft(UIElement element) => (double) element.GetValue(LeftProperty);
        public static void SetLeft(UIElement element, double value) => element.SetValue(LeftProperty, value);
        
        public static double GetTop(UIElement element) => (double) element.GetValue(TopProperty);
        public static void SetTop(UIElement element, double value) => element.SetValue(TopProperty, value);
    }
}
