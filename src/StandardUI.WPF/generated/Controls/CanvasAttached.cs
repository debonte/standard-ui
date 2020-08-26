// This file is generated from ICanvas.cs. Update the source file to change its contents.

using System.StandardUI.Controls;
using System.Windows;

namespace System.StandardUI.Wpf.Controls
{
    public class CanvasAttached : ICanvasAttached
    {
        public static CanvasAttached Instance = new CanvasAttached();
        
        public double GetLeft(IUIElement element) => Canvas.GetLeft((UIElement) element);
        public void SetLeft(IUIElement element, double value) => Canvas.SetLeft((UIElement) element, value);
        
        public double GetTop(IUIElement element) => Canvas.GetTop((UIElement) element);
        public void SetTop(IUIElement element, double value) => Canvas.SetTop((UIElement) element, value);
    }
}
