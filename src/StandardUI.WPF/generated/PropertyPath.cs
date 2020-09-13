// This file is generated from IPropertyPath.cs. Update the source file to change its contents.

using System.StandardUI;

namespace System.StandardUI.Wpf
{
    public class PropertyPath : Windows.DependencyObject, IPropertyPath
    {
        public static readonly Windows.DependencyProperty PathProperty = PropertyUtils.Register(nameof(Path), typeof(string), typeof(PropertyPath), "");
        
        public string Path
        {
            get => (string) GetValue(PathProperty);
        }
    }
}
