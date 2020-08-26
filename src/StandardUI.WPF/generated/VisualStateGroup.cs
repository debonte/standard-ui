// This file is generated from IVisualStateGroup.cs. Update the source file to change its contents.

using System.StandardUI;
using System.Windows;

namespace System.StandardUI.Wpf
{
    public class VisualStateGroup : DependencyObject, IVisualStateGroup
    {
        public static readonly System.Windows.DependencyProperty CurrentStateProperty = PropertyUtils.Register(nameof(CurrentState), typeof(VisualState), typeof(VisualStateGroup), null);
        public static readonly System.Windows.DependencyProperty NameProperty = PropertyUtils.Register(nameof(Name), typeof(string), typeof(VisualStateGroup), "");
        public static readonly System.Windows.DependencyProperty StatesProperty = PropertyUtils.Register(nameof(States), typeof(VisualStateCollection), typeof(VisualStateGroup), null);
        
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
