// This file is generated from IVisualState.cs. Update the source file to change its contents.

using Microsoft.StandardUI;

namespace Microsoft.StandardUI.Wpf
{
    public class VisualState : UIPropertyObject, IVisualState
    {
        public static readonly System.Windows.DependencyProperty NameProperty = PropertyUtils.Register(nameof(Name), typeof(string), typeof(VisualState), "");
        public static readonly System.Windows.DependencyProperty SettersProperty = PropertyUtils.Register(nameof(Setters), typeof(SetterCollection), typeof(VisualState), null);
        
        private SetterCollection _setterCollection;
        
        public VisualState()
        {
            _setterCollection = new SetterCollection();
            SetValue(SettersProperty, _setterCollection);
        }
        
        public string Name => (string) GetValue(NameProperty);
        
        public SetterCollection Setters => _setterCollection;
        ISetterCollection IVisualState.Setters => Setters;
    }
}
