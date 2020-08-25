namespace Microsoft.StandardUI.Controls
{
    public static class CanvasExtensions
    {
        public static void Add(this ICanvas canvas, double left, double top, IUIElement uiElement)
        {
            canvas.Children.Add(uiElement);
            uiElement.SetCanvasLeft(left);
            uiElement.SetCanvasTop(top);
        }
    }
}
