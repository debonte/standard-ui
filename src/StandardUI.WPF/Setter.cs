// This file is generated from ISetter.cs. Update the source file to change its contents.

using Microsoft.StandardUI;
using System.Windows;

namespace Microsoft.StandardUI.Wpf
{
    public class Setter : DependencyObject, ISetter
    {
        public static readonly System.Windows.DependencyProperty PropertyProperty = PropertyUtils.Create(nameof(Property), typeof(DependencyProperty), typeof(DependencyProperty?), null);
        public static readonly System.Windows.DependencyProperty TargetProperty = PropertyUtils.Create(nameof(Target), typeof(TargetPropertyPath), typeof(TargetPropertyPath), null);
        public static readonly System.Windows.DependencyProperty ValueProperty = PropertyUtils.Create(nameof(Value), typeof(object), typeof(object), null);
        
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
