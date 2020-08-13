// This file is generated from ITargetPropertyPath.cs. Update the source file to change its contents.

using Microsoft.StandardUI;
using System.Windows;

namespace Microsoft.StandardUI.Wpf
{
    public class TargetPropertyPath : System.Windows.DependencyObject, ITargetPropertyPath
    {
        public static readonly System.Windows.DependencyProperty PropertyProperty = PropertyUtils.Create(nameof(Property), typeof(PropertyPath), typeof(PropertyPath), null);
        public static readonly System.Windows.DependencyProperty TargetProperty = PropertyUtils.Create(nameof(Target), typeof(object), typeof(object), null);
        
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
