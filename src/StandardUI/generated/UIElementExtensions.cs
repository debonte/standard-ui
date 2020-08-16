// This file is generated from IUIElement.cs. Update the source file to change its contents.

namespace Microsoft.StandardUI
{
    public static class UIElementExtensions
    {
        public static T Width<T>(this T uIElement, double value) where T : IUIElement
        {
            uIElement.Width = value;
            return uIElement;
        }
        
        public static T MinWidth<T>(this T uIElement, double value) where T : IUIElement
        {
            uIElement.MinWidth = value;
            return uIElement;
        }
        
        public static T MaxWidth<T>(this T uIElement, double value) where T : IUIElement
        {
            uIElement.MaxWidth = value;
            return uIElement;
        }
        
        public static T Height<T>(this T uIElement, double value) where T : IUIElement
        {
            uIElement.Height = value;
            return uIElement;
        }
        
        public static T MinHeight<T>(this T uIElement, double value) where T : IUIElement
        {
            uIElement.MinHeight = value;
            return uIElement;
        }
        
        public static T MaxHeight<T>(this T uIElement, double value) where T : IUIElement
        {
            uIElement.MaxHeight = value;
            return uIElement;
        }
    }
}
