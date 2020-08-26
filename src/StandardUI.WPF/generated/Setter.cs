// This file is generated from ISetter.cs. Update the source file to change its contents.

using System.StandardUI;
using System.Windows;

namespace System.StandardUI.Wpf
{
    public class Setter : DependencyObject, ISetter
    {
        public static readonly Windows.DependencyProperty PropertyProperty = PropertyUtils.Register(nameof(Property), typeof(DependencyProperty), typeof(Setter), null);
        public static readonly Windows.DependencyProperty TargetProperty = PropertyUtils.Register(nameof(Target), typeof(TargetPropertyPath), typeof(Setter), null);
        public static readonly Windows.DependencyProperty ValueProperty = PropertyUtils.Register(nameof(Value), typeof(object), typeof(Setter), null);
        
        public DependencyProperty? Property
        {
            get => (DependencyProperty?) GetValue(PropertyProperty);
            set => SetValue(PropertyProperty, value);
        }
        IDependencyProperty? ISetter.Property
        {
            get => Property;
            set => Property = (DependencyProperty?) value;
        }
        
        public TargetPropertyPath Target
        {
            get => (TargetPropertyPath) GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }
        ITargetPropertyPath ISetter.Target
        {
            get => Target;
            set => Target = (TargetPropertyPath) value;
        }
        
        public object Value
        {
            get => (object) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
    }
}
