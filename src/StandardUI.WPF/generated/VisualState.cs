// This file is generated from IVisualState.cs. Update the source file to change its contents.

using System.StandardUI;

namespace System.StandardUI.Wpf
{
    public class VisualState : DependencyObject, IVisualState
    {
        public static readonly Windows.DependencyProperty NameProperty = PropertyUtils.Register(nameof(Name), typeof(string), typeof(VisualState), "");
        public static readonly Windows.DependencyProperty SettersProperty = PropertyUtils.Register(nameof(Setters), typeof(SetterCollection), typeof(VisualState), null);
        
        public string Name
        {
            get => (string) GetValue(NameProperty);
        }
        
        public SetterCollection Setters
        {
            get => (SetterCollection) GetValue(SettersProperty);
        }
        ISetterCollection IVisualState.Setters
        {
            get => Setters;
        }
    }
}
