using System;

namespace System.StandardUI.Controls
{
    public static class CanvasAttachedExtensions
    {
        private static Lazy<ICanvasAttached> LazyCanvasAttached = new Lazy<ICanvasAttached>(() => StandardUIEnvironment.Instance.Factory.CanvasAttached);

        public static ICanvasAttached CanvasAttached => LazyCanvasAttached.Value;

        public static double GetCanvasLeft(this IUIElement element) => CanvasAttached.GetLeft(element);
        public static void SetCanvasLeft(this IUIElement element, double length) => CanvasAttached.SetLeft(element, length);

        public static double GetCanvasTop(this IUIElement element) => CanvasAttached.GetTop(element);
        public static void SetCanvasTop(this IUIElement element, double length) => CanvasAttached.SetTop(element, length);
    }
}
