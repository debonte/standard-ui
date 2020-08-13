// This file is generated from ICanvas.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using System.Windows;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class Canvas : Panel, ICanvas
    {
        public static readonly System.Windows.DependencyProperty LeftProperty = AttachedPropertyUtils.Create("Left", typeof(double), typeof(UIElement), 0.0);
        public static readonly System.Windows.DependencyProperty TopProperty = AttachedPropertyUtils.Create("Top", typeof(double), typeof(UIElement), 0.0);
        
        public static double GetLeft(UIElement element) => (double) element.GetValue(LeftProperty);
        public static void SetLeft(UIElement element, double value) => element.SetValue(LeftProperty, value);
        
        public static double GetTop(UIElement element) => (double) element.GetValue(TopProperty);
        public static void SetTop(UIElement element, double value) => element.SetValue(TopProperty, value);
    }
}
