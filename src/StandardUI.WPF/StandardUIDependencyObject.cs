using System.Windows;
using System.Windows.Media;

namespace Microsoft.StandardUI.Wpf
{
    /// <summary>
    /// This is the base for predefined dependency objects;
    /// </summary>
    public class StandardUIDependencyObject : DependencyObject, IUIPropertyObject
    {
        public object GetValue(IUIProperty property)
        {
            DependencyProperty dependencyProperty = ((UIProperty)property).DependencyProperty;
            return GetValue(dependencyProperty);
        }

        public object ReadLocalValue(IUIProperty property)
        {
            DependencyProperty dependencyProperty = ((UIProperty)property).DependencyProperty;
            return ReadLocalValue(dependencyProperty);
        }

        public void SetValue(IUIProperty property, object value)
        {
            DependencyProperty dependencyProperty = ((UIProperty)property).DependencyProperty;
            SetValue(dependencyProperty, value);
        }
    }
}
