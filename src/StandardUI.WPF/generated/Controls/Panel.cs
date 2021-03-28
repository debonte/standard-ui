// This file is generated from IPanel.cs. Update the source file to change its contents.

using System.StandardUI.Controls;

namespace System.StandardUI.Wpf.Controls
{
    public class Panel : StandardUIFrameworkElement, IPanel
    {
        public static readonly Windows.DependencyProperty ChildrenProperty = PropertyUtils.Register(nameof(Children), typeof(UIElementCollection), typeof(Panel), null);
        
        private UIElementCollection _uiElementCollection;
        
        public Panel()
        {
            _uiElementCollection = new UIElementCollection(this);
            SetValue(ChildrenProperty, _uiElementCollection);
        }
        
        public UIElementCollection Children => _uiElementCollection;
        IUIElementCollection IPanel.Children => Children;
        
        protected override int VisualChildrenCount => _uiElementCollection.Count;
        
        protected override Windows.Media.Visual GetVisualChild(int index) => (Windows.Media.Visual) _uiElementCollection[index];
    }
}
