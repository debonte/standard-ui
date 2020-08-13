// This file is generated from IVisualState.cs. Update the source file to change its contents.

using Microsoft.StandardUI;
using System.Windows;

namespace Microsoft.StandardUI.Wpf
{
    public class VisualState : DependencyObject, IVisualState
    {
        public static readonly System.Windows.DependencyProperty NameProperty = PropertyUtils.Create(nameof(Name), typeof(string), typeof(string), "");
        public static readonly System.Windows.DependencyProperty SettersProperty = PropertyUtils.Create(nameof(Setters), typeof(SetterCollection), typeof(SetterCollection), null);
        
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
