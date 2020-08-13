// This file is generated from ICanvas.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using System.Windows;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class CanvasAttached : ICanvasAttached
    {
        
        public double GetLeft(UIElement element) => Canvas.GetLeft(element);
        public void SetLeft(UIElement element, double value) => Canvas.SetLeft(element, value);
        
        public double GetTop(UIElement element) => Canvas.GetTop(element);
        public void SetTop(UIElement element, double value) => Canvas.SetTop(element, value);
    }
}
