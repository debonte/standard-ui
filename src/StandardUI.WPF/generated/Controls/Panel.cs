// This file is generated from IPanel.cs. Update the source file to change its contents.

using System.StandardUI.Controls;
using System.Windows;
using System.Windows.Media;

namespace System.StandardUI.Wpf.Controls
{
    public class Panel : StandardUIElement, IPanel
    {
        public static readonly Windows.DependencyProperty ChildrenProperty = PropertyUtils.Register(nameof(Children), typeof(UIElementCollection), typeof(Panel), null);

        private UIElementCollection _uiElementCollection;

        public Panel()
        {
            _uiElementCollection = new UIElementCollection(this);
            SetValue(ChildrenProperty, _uiElementCollection);
        }
        
        public UIElementCollection Children
        {
            get => (UIElementCollection) GetValue(ChildrenProperty);
        }
        IUIElementCollection IPanel.Children
        {
            get => Children;
        }

        protected override int VisualChildrenCount => _uiElementCollection.Count;

        protected override Visual GetVisualChild(int index) => (Visual)_uiElementCollection[index];
    }
}
