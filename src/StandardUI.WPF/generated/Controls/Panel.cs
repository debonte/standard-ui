// This file is generated from IPanel.cs. Update the source file to change its contents.

using System.StandardUI.Controls;
using System.Windows;

namespace System.StandardUI.Wpf.Controls
{
    public class Panel : UIElement, IPanel
    {
        public static readonly System.Windows.DependencyProperty ChildrenProperty = PropertyUtils.Register(nameof(Children), typeof(UIElementCollection), typeof(Panel), null);
        
        public UIElementCollection Children
        {
            get => (UIElementCollection) GetValue(ChildrenProperty);
        }
        IUIElementCollection IPanel.Children
        {
            get => Children;
        }
    }
}
