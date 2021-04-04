// This file is generated from IPropertyPath.cs. Update the source file to change its contents.

using Microsoft.StandardUI;

namespace Microsoft.StandardUI.Wpf
{
    public class PropertyPath : StandardUIDependencyObject, IPropertyPath
    {
        public static readonly System.Windows.DependencyProperty PathProperty = PropertyUtils.Register(nameof(Path), typeof(string), typeof(PropertyPath), "");
        
        public string Path => (string) GetValue(PathProperty);
    }
}
