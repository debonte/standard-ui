// This file is generated from IPanel.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;
using System.Windows;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class Panel : UIElement, IPanel
    {
        public static readonly System.Windows.DependencyProperty ChildrenProperty = PropertyUtils.Create(nameof(Children), typeof(UIElementCollection), typeof(UIElementCollection), null);
        
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
