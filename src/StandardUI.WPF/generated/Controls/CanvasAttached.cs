// This file is generated from ICanvas.cs. Update the source file to change its contents.

using System.StandardUI.Controls;

namespace System.StandardUI.Wpf.Controls
{
    public class CanvasAttached : ICanvasAttached
    {
        public static CanvasAttached Instance = new CanvasAttached();
        
        public double GetLeft(IUIElement element) => Canvas.GetLeft((Windows.UIElement) element);
        public void SetLeft(IUIElement element, double value) => Canvas.SetLeft((Windows.UIElement) element, value);
        
        public double GetTop(IUIElement element) => Canvas.GetTop((Windows.UIElement) element);
        public void SetTop(IUIElement element, double value) => Canvas.SetTop((Windows.UIElement) element, value);
    }
}
