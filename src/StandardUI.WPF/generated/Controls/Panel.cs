// This file is generated from IPanel.cs. Update the source file to change its contents.

using Microsoft.StandardUI.Controls;

namespace Microsoft.StandardUI.Wpf.Controls
{
    public class Panel : StandardUIFrameworkElement, IPanel
    {
        public static readonly System.Windows.DependencyProperty ChildrenProperty = PropertyUtils.Register(nameof(Children), typeof(UIElementCollection), typeof(Panel), null);
        
        private UIElementCollection _uiElementCollection;
        
        public Panel()
        {
            _uiElementCollection = new UIElementCollection(this);
            SetValue(ChildrenProperty, _uiElementCollection);
        }
        
        public UIElementCollection Children => _uiElementCollection;
        IUIElementCollection IPanel.Children => Children;
        
        protected override int VisualChildrenCount => _uiElementCollection.Count;
        
        protected override System.Windows.Media.Visual GetVisualChild(int index) => (System.Windows.Media.Visual) _uiElementCollection[index];
    }
}
