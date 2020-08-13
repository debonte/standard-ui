// This file is generated from IVisualStateGroup.cs. Update the source file to change its contents.

using Microsoft.StandardUI;
using System.Windows;

namespace Microsoft.StandardUI.Wpf
{
    public class VisualStateGroup : DependencyObject, IVisualStateGroup
    {
        public static readonly System.Windows.DependencyProperty CurrentStateProperty = PropertyUtils.Create(nameof(CurrentState), typeof(VisualState), typeof(VisualState), null);
        public static readonly System.Windows.DependencyProperty NameProperty = PropertyUtils.Create(nameof(Name), typeof(string), typeof(string), "");
        public static readonly System.Windows.DependencyProperty StatesProperty = PropertyUtils.Create(nameof(States), typeof(VisualStateCollection), typeof(VisualStateCollection), null);
        
        public VisualState CurrentState
        {
            get => (VisualState) GetValue(CurrentStateProperty);
        }
        IVisualState IVisualStateGroup.CurrentState
        {
            get => CurrentState;
        }
        
        public string Name
        {
            get => (string) GetValue(NameProperty);
        }
        
        public VisualStateCollection States
        {
            get => (VisualStateCollection) GetValue(StatesProperty);
        }
        IVisualStateCollection IVisualStateGroup.States
        {
            get => States;
        }
    }
}
