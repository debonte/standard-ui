using System.Windows;

namespace Microsoft.StandardUI.Wpf
{
    public class UIProperty : IUIProperty
    {
        public DependencyProperty DependencyProperty { get;  }

        public UIProperty(DependencyProperty property)
        {
            DependencyProperty = property;
        }

        public static DependencyProperty GetDependencyProeprty(IUIProperty property)
        {
            return ((UIProperty)property).DependencyProperty;
        }
    }
}
