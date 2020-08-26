// This file is generated from ITargetPropertyPath.cs. Update the source file to change its contents.

using System.StandardUI;
using System.Windows;

namespace System.StandardUI.Wpf
{
    public class TargetPropertyPath : Windows.DependencyObject, ITargetPropertyPath
    {
        public static readonly Windows.DependencyProperty PropertyProperty = PropertyUtils.Register(nameof(Property), typeof(PropertyPath), typeof(TargetPropertyPath), null);
        public static readonly Windows.DependencyProperty TargetProperty = PropertyUtils.Register(nameof(Target), typeof(object), typeof(TargetPropertyPath), null);
        public PropertyPath Property
        {
            get => (PropertyPath) GetValue(PropertyProperty);
            set => SetValue(PropertyProperty, value);
        }
        IPropertyPath ITargetPropertyPath.Property
        {
            get => Property;
            set => Property = (PropertyPath) value;
        }
        
        public object Target
        {
            get => (object) GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }
    }
}
