// This file is generated from IVisualStateGroup.cs. Update the source file to change its contents.

using System.StandardUI;

namespace System.StandardUI.Wpf
{
    public class VisualStateGroup : DependencyObject, IVisualStateGroup
    {
        public static readonly Windows.DependencyProperty CurrentStateProperty = PropertyUtils.Register(nameof(CurrentState), typeof(VisualState), typeof(VisualStateGroup), null);
        public static readonly Windows.DependencyProperty NameProperty = PropertyUtils.Register(nameof(Name), typeof(string), typeof(VisualStateGroup), "");
        public static readonly Windows.DependencyProperty StatesProperty = PropertyUtils.Register(nameof(States), typeof(VisualStateCollection), typeof(VisualStateGroup), null);
        
        private VisualStateCollection _visualStateCollection;
        
        public VisualStateGroup()
        {
            _visualStateCollection = new VisualStateCollection();
            SetValue(StatesProperty, _visualStateCollection);
        }
        
        public VisualState CurrentState => (VisualState) GetValue(CurrentStateProperty);
        IVisualState IVisualStateGroup.CurrentState => CurrentState;
        
        public string Name => (string) GetValue(NameProperty);
        
        public VisualStateCollection States => _visualStateCollection;
        IVisualStateCollection IVisualStateGroup.States => States;
    }
}
